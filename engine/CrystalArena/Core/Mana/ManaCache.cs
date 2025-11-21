using System;
using J2N.Collections.Generic.Extensions;

namespace CrystalArena
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;

    [Copyable]
    public class ManaCache
    {
        private readonly Player _controller;
        private readonly List<TrackableList<ManaUnit>> _groups;
        private readonly TrackableList<ManaUnit> _manaPool = new TrackableList<ManaUnit>();
        private readonly object _manaPoolCountLock = new object();
        private readonly TrackableList<ManaUnit> _removeList = new TrackableList<ManaUnit>();
        private readonly TrackableList<ManaUnit> _units = new TrackableList<ManaUnit>();
        private readonly TrackableList<ManaUnit> _crystalUnits = new TrackableList<ManaUnit>();

        private ManaCache() { }

        public ManaCache(Player controller)
        {
            _controller = controller;
            _groups = new List<TrackableList<ManaUnit>>
            {
                new TrackableList<ManaUnit>(),
                new TrackableList<ManaUnit>(),
                new TrackableList<ManaUnit>(),
                new TrackableList<ManaUnit>(),
                new TrackableList<ManaUnit>(),
                new TrackableList<ManaUnit>(),
                new TrackableList<ManaUnit>(),
                new TrackableList<ManaUnit>(),
                _units,
                _crystalUnits,
            };
        }

        private const int ColorlessUnitIndex = 8;
        private const int CrystalUnitIndex = 9;

        public ManaCounts ManaPool
        {
            get
            {
                // this is accessed from a timer thread, which refreshes ui
                // if a call to empty mana pool is made at the same time
                // the collection will be modified and an exception will be thrown,
                // a lock is needed to prevent this.
                lock (_manaPoolCountLock)
                {
                    return new ManaCounts(
                        light: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsWhite),
                        water: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsBlue),
                        dark: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsBlack),
                        fire: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsRed),
                        wind: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsGreen),
                        ice: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsIce),
                        earth: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsEarth),
                        lightning: _manaPool.Count(x => !x.Color.IsMulti && x.Color.IsPurple),
                        multi: _manaPool.Count(x => x.Color.IsMulti),
                        colorless: _manaPool.Count(x => x.Color.IsColorless),
                        crystal: _manaPool.Count(x => x.Color.IsCrystal)
                    );
                }
            }
        }

        public void Initialize(INotifyChangeTracker changeTracker)
        {
            foreach (var managroup in _groups)
            {
                managroup.Initialize(changeTracker);
            }

            _manaPool.Initialize(changeTracker);
            _removeList.Initialize(changeTracker);
        }

        public void AddManaToPool(ManaAmount amount, ManaUsage usage)
        {
            lock (_manaPoolCountLock)
            {
                foreach (var mana in amount)
                {
                    for (var i = 0; i < mana.Count; i++)
                    {
                        var unit = new ManaUnit(mana.Color, 0, usageRestriction: usage);
                        Add(unit);

                        _manaPool.Add(unit);
                    }
                }
            }
        }

        private List<ManaUnit> GetAdditionalManaSources(ConvokeAndDelveOptions convokeAndDelve)
        {
            var additional = new List<ManaUnit>();
            convokeAndDelve = convokeAndDelve ?? ConvokeAndDelveOptions.NoConvokeAndDelve;

            if (convokeAndDelve.CanUseConvoke)
            {
                additional.AddRange(GetConvokeSources(convokeAndDelve.UiConvokeSources));
            }

            if (convokeAndDelve.CanUseDelve)
            {
                additional.AddRange(GetDelveSources(convokeAndDelve.UiDelveSources));
            }

            additional.AddRange(GetDiscardableManaSources());

            return additional;
        }

        private IEnumerable<ManaUnit> GetDiscardableManaSources()
        {
            return new List<ManaUnit>();
            //return _controller.Hand.Select(x => x.Abil)
        }

        public List<ManaColor> GetAvailableMana(
            ManaUsage usage,
            ConvokeAndDelveOptions convokeAndDelve
        )
        {
            var restricted = new HashSet<ManaUnit>();
            var allocated = new List<ManaUnit>();

            var units = _units.Concat(GetAdditionalManaSources(convokeAndDelve)).ToList();

            foreach (var manaUnit in units)
            {
                if (IsAvailable(manaUnit, restricted, usage))
                {
                    restricted.Add(manaUnit);
                    allocated.Add(manaUnit);

                    RestrictUsingDifferentSourcesFromSameCard(manaUnit, restricted, units);
                }
            }

            return allocated.Select(x => x.Color).ToList();
        }

        private IEnumerable<ManaUnit> GetConvokeSources(List<Card> uiSelected)
        {
            // first candidates are the one that were selected by ui
            // if any
            var convokeSources = new List<ConvokeManaSource>();

            if (uiSelected?.Count > 0)
            {
                // rank should be lower that 0 so it will be used before mana from pool
                convokeSources.AddRange(uiSelected.Select(x => new ConvokeManaSource(x, rank: -1)));
            }

            convokeSources.AddRange(
                _controller
                    .Battlefield.Forwards.OrderBy(x => x.Power)
                    .Where(x => !convokeSources.Any(y => y.OwningCard == x))
                    .Select(x => new ConvokeManaSource(x))
            );

            return convokeSources.SelectMany(x => x.GetUnits()).ToList();
        }

        private IEnumerable<ManaUnit> GetDelveSources(List<Card> uiSelected)
        {
            // first candidates are the one that were selected by ui
            // if any
            var convokeSources = new List<DelveManaSource>();

            if (uiSelected?.Count > 0)
            {
                // rank should be lower that 0 so it will be used before mana from pool
                convokeSources.AddRange(uiSelected.Select(x => new DelveManaSource(x, rank: -1)));
            }

            convokeSources.AddRange(
                _controller
                    .BreakZone.OrderBy(x => x.Score)
                    .Where(x => !convokeSources.Any(y => y.OwningCard == x))
                    .Select(x => new DelveManaSource(x))
            );

            return convokeSources.SelectMany(x => x.GetUnits()).ToList();
        }

        private void RestrictUsingDifferentSourcesFromSameCard(
            ManaUnit allocated,
            HashSet<ManaUnit> restricted,
            IEnumerable<ManaUnit> units
        )
        {
            // Same card cannot be tapped twice, therefore multiple sources
            // from same card cannot be used simultaniously.

            // Add to restricted all units produced by another source
            // of the same card.
            var unitsProducedByAnotherSourceOnSameCard = units.Where(unit =>
                unit.HasSource
                && allocated.HasSource
                && unit.Source.OwningCard == allocated.Source.OwningCard
                && unit.Source != allocated.Source
            );

            foreach (var unit in unitsProducedByAnotherSourceOnSameCard)
            {
                restricted.Add(unit);
            }
        }

        public void EmptyManaPool()
        {
            foreach (var unit in _manaPool.Where(x => !x.HasSource))
            {
                if (!unit.Color.IsCrystal)
                {
                    RemovePermanently(unit);
                }
            }

            lock (_manaPoolCountLock)
            {
                _manaPool.RemoveAll(x => !x.Color.IsCrystal);
            }

            RemoveAllScheduled();
        }

        public void Add(ManaUnit unit)
        {
            if (unit.Color.IsCrystal)
            {
                _crystalUnits.Add(unit);
                return;
            }

            foreach (var colorIndex in unit.Color.Indices)
            {
                _groups[colorIndex].Add(unit);
            }

            // Every mana can be used as colorless.
            // True colorless mana was already added, so we don't add it again.
            if (unit.Color.IsColorless == false)
            {
                _units.Add(unit);
            }
        }

        public void Remove(ManaUnit unit)
        {
            if (_manaPool.Contains(unit))
            {
                _removeList.Add(unit);
                return;
            }

            RemovePermanently(unit);
        }

        public bool Has(
            ManaAmount amount,
            ManaUsage usage,
            ConvokeAndDelveOptions convokeAndDelveOptions
        )
        {
            var allocated = TryToAllocateAmount(amount, usage, convokeAndDelveOptions);

            return allocated != null && allocated.Lifeloss < _controller.Life;
        }

        public void Consume(
            ManaAmount amount,
            ManaUsage usage,
            ConvokeAndDelveOptions convokeAndDelveOptions
        )
        {
            var allocated = TryToAllocateAmount(amount, usage, convokeAndDelveOptions);
            Asrt.True(allocated != null, "Not enough mana available.");

            var sources = GetSourcesToActivate(allocated.Units);

            foreach (var source in sources)
            {
                lock (_manaPoolCountLock)
                {
                    foreach (var unit in source.GetUnits())
                    {
                        _manaPool.Add(unit);
                    }
                }

                source.PayActivationCost();
            }

            lock (_manaPoolCountLock)
            {
                foreach (var unit in allocated.Units)
                {
                    _manaPool.Remove(unit);
                }
            }

            foreach (var unit in allocated.Units.Where(x => !x.HasSource))
            {
                RemovePermanently(unit);
            }

            _controller.Life -= allocated.Lifeloss;

            Asrt.True(NonCrystalManaPoolCount() <= 1, "Mana was not consumed completely.");

            EmptyManaPool();
        }

        public int NonCrystalManaPoolCount()
        {
            return _manaPool.Count(x => !x.Color.IsCrystal);
        }

        private void RemoveAllScheduled()
        {
            foreach (var unit in _removeList)
            {
                RemovePermanently(unit);
            }
            _removeList.Clear();
        }

        private void RemovePermanently(ManaUnit unit)
        {
            foreach (var colorIndex in unit.Color.Indices)
            {
                _groups[colorIndex].Remove(unit);
            }

            _units.Remove(unit);
        }

        private bool IsAvailable(ManaUnit unit, HashSet<ManaUnit> restricted, ManaUsage usage)
        {
            if (restricted.Contains(unit))
                return false;

            if (unit.CanActivateSource() == false && _manaPool.Contains(unit) == false)
                return false;

            if (unit.CanBeUsed(usage) == false)
                return false;

            return true;
        }

        private class AllocatedAmount
        {
            public readonly HashSet<ManaUnit> Units = new HashSet<ManaUnit>();
            public int Lifeloss;

            public int NonCrystalTotalCount()
            {
                return Units.Count(x => !x.Color.IsCrystal);
            }
        }

        private AllocatedAmount TryToAllocateAmount(
            ManaAmount amount,
            ManaUsage usage,
            ConvokeAndDelveOptions convokeAndDelveOptions
        )
        {
            var restricted = new HashSet<ManaUnit>();
            var allocated = new AllocatedAmount();

            var additional = GetAdditionalManaSources(convokeAndDelveOptions);
            var additionalGrouped = GroupAdditionalSources(additional);
            var units = _units.Concat(additional).ToList();

            var checkAmount = amount
                .Select(x => new
                {
                    Color = GetColorIndex(x),
                    Count = x.Count,
                    IsPhyrexian = x.Color.IsPhyrexian,
                    IsCrystal = x.Color.IsCrystal,
                })
                // first check for mana which has only few mana sources
                .OrderBy(x => _groups[x.Color].Count)
                .ToArray();

            var allocatedPhyrexian = new List<ManaUnit>();

            foreach (var manaOfSingleColor in checkAmount)
            {
                var ordered = _groups[manaOfSingleColor.Color]
                    .Concat(additionalGrouped[manaOfSingleColor.Color])
                    .OrderBy(GetManaUnitAllocationOrder)
                    .ToList();

                for (var i = 0; i < manaOfSingleColor.Count; i++)
                {
                    var allocatedUnit = ordered.FirstOrDefault(unit =>
                        IsAvailable(unit, restricted, usage)
                    );

                    // allocation failed
                    if (allocatedUnit == null)
                    {
                        if (manaOfSingleColor.IsPhyrexian)
                        {
                            allocated.Lifeloss += 2;
                            continue;
                        }

                        // if pyrexian is holding the slot, release it and pay life
                        if (allocatedPhyrexian.Count > 0)
                        {
                            var restrictedWithoutPhyrexian = restricted
                                .Where(x => !allocatedPhyrexian.Contains(x))
                                .ToHashSet();
                            allocatedUnit = ordered.FirstOrDefault(unit =>
                                IsAvailable(unit, restrictedWithoutPhyrexian, usage)
                            );

                            if (allocatedUnit != null)
                            {
                                allocated.Lifeloss += 2;
                                allocatedPhyrexian.Remove(allocatedUnit);
                                continue;
                            }
                        }

                        return null;
                    }

                    if (manaOfSingleColor.IsPhyrexian)
                    {
                        allocatedPhyrexian.Add(allocatedUnit);
                    }

                    restricted.Add(allocatedUnit);
                    allocated.Units.Add(allocatedUnit);

                    RestrictUsingDifferentSourcesFromSameCard(allocatedUnit, restricted, units);
                }
            }

            if (!AllocatedAmountConsumesAllNonCrystalMana(allocated))
            {
                return null;
            }

            return allocated;
        }

        private bool AllocatedAmountConsumesAllNonCrystalMana(AllocatedAmount allocated)
        {
            var allocatedCount = allocated.NonCrystalTotalCount();
            var poolCount = NonCrystalManaPoolCount();
            return (poolCount == 0)
                || (allocatedCount == poolCount)
                || (allocatedCount == poolCount - 1);
        }

        private List<ManaUnit>[] GroupAdditionalSources(List<ManaUnit> additional)
        {
            var additionalGrouped = new[]
            {
                new List<ManaUnit>(),
                new List<ManaUnit>(),
                new List<ManaUnit>(),
                new List<ManaUnit>(),
                new List<ManaUnit>(),
                new List<ManaUnit>(),
                new List<ManaUnit>(),
                new List<ManaUnit>(),
                new List<ManaUnit>(),
                new List<ManaUnit>(),
            };

            if (additional != null)
            {
                foreach (var unit in additional)
                {
                    foreach (var color in unit.Color.Indices)
                    {
                        additionalGrouped[color].Add(unit);
                    }

                    // Every mana can be used as colorless.
                    // True colorless mana was already added, so we don't add it again.
                    if (!unit.Color.IsColorless && !unit.Color.IsCrystal)
                    {
                        additionalGrouped[ColorlessUnitIndex].Add(unit);
                    }
                }
            }
            return additionalGrouped;
        }

        private int GetColorIndex(SingleColorManaAmount manaOfSingleColor)
        {
            if (manaOfSingleColor.Color.IsCrystal)
                return CrystalUnitIndex;
            return manaOfSingleColor.Color.IsColorless
                ? ColorlessUnitIndex
                // amount never contains mana whish is multiple colors at the same time
                : manaOfSingleColor.Color.Indices[0];
        }

        private int GetManaUnitAllocationOrder(ManaUnit x)
        {
            return _manaPool.Contains(x) ? 0 : x.Rank * 10 + x.Color.Indices.Count;
        }

        private List<IManaSource> GetSourcesToActivate(IEnumerable<ManaUnit> units)
        {
            return units
                .Where(x => x.HasSource)
                .Where(x => !_manaPool.Contains(x))
                .GroupBy(x => x.Source)
                .Select(x => x.Key)
                .ToList();
        }
    }
}
