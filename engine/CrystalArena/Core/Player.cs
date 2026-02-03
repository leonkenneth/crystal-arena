namespace CrystalArena
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Decisions;
    using Events;
    using Infrastructure;
    using Modifiers;

    public class Player : GameObject, ITarget, IDamageable, IHasLife, IModifiable
    {
        public readonly ManaCache ManaCache;
        private readonly Battlefield _battlefield;
        private readonly ContiniousEffects _continiousEffects = new ContiniousEffects();
        private readonly Deck _deck;
        private readonly RemovedFromPlay _removedFromPlay;
        private readonly DamageZone _damageZone;
        private readonly BreakZone _breakZone;
        private readonly Hand _hand;
        private readonly Trackable<bool> _hasLost = new Trackable<bool>();
        private readonly Trackable<bool> _hasMulligan = new Trackable<bool>(true);
        private readonly Trackable<bool> _hasPriority = new Trackable<bool>();
        private readonly Trackable<bool> _isActive = new Trackable<bool>();
        private readonly BackupLimit _backupLimit = new BackupLimit(7);
        private readonly Trackable<int> _backupsPlayedCount = new Trackable<int>(0);
        private readonly MainDeck _library;
        private readonly LimitBreak _limitBreak;
        private readonly Life _life = new Life(7);
        private readonly TrackableList<IPlayerModifier> _modifiers =
            new TrackableList<IPlayerModifier>();
        private readonly TrackableList<Emblem> _emblems = new TrackableList<Emblem>();
        private readonly SkipSteps _skipSteps = new SkipSteps();

        public Player(PlayerParameters p, PlayerType controllerType)
        {
            Name = p.Name;
            AvatarId = p.AvatarId;
            Type = controllerType;

            ManaCache = new ManaCache(this);
            _battlefield = new Battlefield(this);
            _hand = new Hand(this);
            _breakZone = new BreakZone(this);
            _library = new MainDeck(this);
            _limitBreak = new LimitBreak(this);
            _removedFromPlay = new RemovedFromPlay(this);
            _damageZone = new DamageZone(this);
            _deck = p.Deck;
        }

        private Player() { }

        public PlayerType Type { get; private set; }
        public string Name { get; private set; }

        public Player Opponent
        {
            get { return Players.GetOpponent(this); }
        }

        public int BackupsPlayedCount
        {
            get { return _backupsPlayedCount.Value; }
            set { _backupsPlayedCount.Value = value; }
        }

        private IEnumerable<IAcceptsPlayerModifier> ModifiableProperties
        {
            get
            {
                yield return _backupLimit;
                yield return _continiousEffects;
                yield return _skipSteps;
            }
        }

        public int AvatarId { get; private set; }

        public Deck Deck
        {
            get { return _deck; }
        }

        public IBattlefieldQuery Battlefield
        {
            get { return _battlefield; }
        }

        public IZoneQuery RemovedFromPlay
        {
            get { return _removedFromPlay; }
        }

        public bool CanMulligan
        {
            get { return !HasAlreadyMulliganed; }
        }

        public bool CanPlayBackups
        {
            get { return BackupsPlayedCount < _backupLimit.Value; }
        }

        public IBreakZoneQuery BreakZone
        {
            get { return _breakZone; }
        }

        public IHandQuery Hand
        {
            get { return _hand; }
        }
        public LimitBreak LimitBreak
        {
            get { return _limitBreak; }
        }

        public bool HasLost
        {
            get { return _hasLost.Value; }
            set { _hasLost.Value = value; }
        }

        public bool HasAlreadyMulliganed
        {
            get { return _hasMulligan.Value; }
            set { _hasMulligan.Value = value; }
        }

        public bool HasPriority
        {
            get { return _hasPriority.Value; }
            set { _hasPriority.Value = value; }
        }

        public virtual bool IsActive
        {
            get { return _isActive.Value; }
            set { _isActive.Value = value; }
        }

        public bool IsHuman
        {
            get { return Type == PlayerType.Human; }
        }

        public bool IsMachine
        {
            get { return Type == PlayerType.Machine; }
        }

        public bool IsScenario
        {
            get { return Type == PlayerType.Scenario; }
        }

        public bool IsMax { get; set; }

        public IMainDeckQuery MainDeck
        {
            get { return _library; }
        }

        public bool HasAttackedThisTurn
        {
            get { return IsActive && Game.Turn.Events.HasActivePlayerAttackedThisTurn; }
        }

        public int NumberOfCardsAboveMaximumHandSize
        {
            get { return Math.Max(0, _hand.Count - 5); }
        }

        public IEnumerable<Emblem> Emblems
        {
            get { return _emblems; }
        }

        public int Score
        {
            get
            {
                var score =
                    _life.Score
                    + _battlefield.Score
                    + _hand.Score
                    + _breakZone.Score
                    + _emblems.Sum(x => x.Score);

                if (HasLost)
                {
                    score -= (1000 - Turn.TurnCount) * 10000;
                }

                return IsMax ? score : -score;
            }
        }

        public void ReceiveDamage(IDamage damage)
        {
            var singleDamage = damage as Damage;
            var aggregateDamage = damage as AggregateDamage;

            if (aggregateDamage != null)
            {
                aggregateDamage.Damages.ToList().ForEach(AdjustPreventedDamage);
            }
            else
            {
                AdjustPreventedDamage(singleDamage!);
            }

            if (damage.Amount == 0)
                return;

            if (aggregateDamage != null)
            {
                aggregateDamage.Damages = aggregateDamage.Damages.Where(d =>
                    !Game.RedirectDamage(d, this)
                );
            }
            else
            {
                var wasRedirected = Game.RedirectDamage(damage, this);

                if (wasRedirected)
                    return;
            }

            if (damage.Amount == 0) // If all aggregated damage was redirected
                return;

            var preventedLifeloss = Game.PreventLifeloss(damage.Amount, this, queryOnly: false);

            var dealtAmount = (damage.Amount - preventedLifeloss);
            Life -= dealtAmount;
            MillXCardToDamageZone(dealtAmount);

            Publish(new DamageDealtEvent(this, damage));
        }

        private void MillXCardToDamageZone(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var milledCard = MainDeck.Top;
                if (milledCard.HasExBurst)
                {
                    Enqueue(new ChooseToUseExBurst(this, milledCard.GetExBurstEffectSource()));
                }
                PutCardToDamageZone(milledCard);
            }
        }

        private void AdjustPreventedDamage(Damage damage)
        {
            var p = new PreventDamageParameters
            {
                Amount = damage.Amount,
                Source = damage.Source,
                Target = this,
                IsCombat = damage.IsCombat,
                CanBePrevented = damage.CanBePrevented,
                QueryOnly = false,
            };

            var prevented = Game.PreventDamage(p);
            damage.Amount -= prevented;
        }

        public int Life
        {
            get { return _life.Value; }
            set
            {
                var oldValue = _life.Value;

                _life.Value = value;

                if (Life <= 0)
                    HasLost = true;

                Publish(new LifeChangedEvent(this, value, oldValue));
            }
        }

        void IModifiable.RemoveModifier(IModifier modifier)
        {
            RemoveModifier((IPlayerModifier)modifier);
        }

        public int Id { get; private set; }
        public IZoneQuery DamageZone
        {
            get { return _damageZone; }
        }

        public int CalculateHash(HashCalculator calc)
        {
            return HashCalculator.Combine(
                Life,
                HasPriority.GetHashCode(),
                IsActive.GetHashCode(),
                calc.Calculate(_battlefield),
                calc.Calculate(_breakZone),
                calc.Calculate(_library),
                calc.Calculate(_hand),
                calc.Calculate(_removedFromPlay),
                calc.Calculate(_damageZone),
                _backupLimit.Value.GetValueOrDefault(),
                _backupsPlayedCount.Value
            );
        }

        public int CalculatePreventedReceivedDamageAmount(
            int totalAmount,
            Card source,
            bool isCombat = false
        )
        {
            var p = new PreventDamageParameters
            {
                Amount = totalAmount,
                Source = source,
                Target = this,
                IsCombat = isCombat,
                QueryOnly = true,
            };

            return Game.PreventDamage(p);
        }

        public void AddModifier(IPlayerModifier modifier, ModifierParameters p)
        {
            p.Owner = this;
            _modifiers.Add(modifier);

            modifier.Initialize(p, Game);
            modifier.Activate();

            foreach (var modifiableProperty in ModifiableProperties)
            {
                modifiableProperty.Accept(modifier);
            }
        }

        public void RemoveModifier(IPlayerModifier modifier)
        {
            _modifiers.Remove(modifier);
            modifier.Dispose();
        }

        public void Initialize(Game game)
        {
            Game = game;
            Id = game.Recorder.CreateId(this);

            _life.Initialize(ChangeTracker);
            _backupsPlayedCount.Initialize(ChangeTracker);
            _hasMulligan.Initialize(ChangeTracker);
            _hasLost.Initialize(ChangeTracker);
            _isActive.Initialize(ChangeTracker);
            _hasPriority.Initialize(ChangeTracker);
            ManaCache.Initialize(ChangeTracker);
            _modifiers.Initialize(ChangeTracker);
            _continiousEffects.Initialize(null, Game);
            _backupLimit.Initialize(Game, null);
            _battlefield.Initialize(Game);
            _hand.Initialize(Game);
            _breakZone.Initialize(Game);
            _library.Initialize(Game);
            _removedFromPlay.Initialize(Game);
            _damageZone.Initialize(Game);
            _skipSteps.Initialize(ChangeTracker);
            _emblems.Initialize(ChangeTracker);

            new PlayerDeckLoader(Game, this, _deck).LoadDeck();
        }

        public void PutCardToBattlefield(Card card)
        {
            _battlefield.Add(card);
        }

        public int GetAvailableManaCount(
            ConvokeAndDelveOptions convokeAndDelveOptions = null,
            ManaUsage usage = ManaUsage.Any
        )
        {
            return GetAvailableMana(convokeAndDelveOptions, usage).Count;
        }

        public List<ManaColor> GetAvailableMana(
            ConvokeAndDelveOptions convokeAndDelveOptions = null,
            ManaUsage usage = ManaUsage.Any
        )
        {
            return ManaCache.GetAvailableMana(usage, convokeAndDelveOptions);
        }

        public void AddManaToManaPool(
            ManaAmount manaAmount,
            ManaUsage usageRestriction = ManaUsage.Any
        )
        {
            ManaCache.AddManaToPool(manaAmount, usageRestriction);
        }

        public void Consume(
            ManaAmount amount,
            ManaUsage usage,
            ConvokeAndDelveOptions convokeAndDelveOptions = null
        )
        {
            ManaCache.Consume(amount, usage, convokeAndDelveOptions);
        }

        public void DiscardCard(Card card)
        {
            _breakZone.AddToEnd(card);

            Publish(new PlayerDiscardsCardEvent(this, card));
        }

        public void DiscardHand()
        {
            foreach (var card in _hand.ToList())
            {
                DiscardCard(card);
            }
        }

        public Card DiscardRandomCard()
        {
            if (_hand.IsEmpty)
                return null;

            var card = _hand.RandomCard;
            DiscardCard(card);
            return card;
        }

        public void DrawCard()
        {
            var card = _library.Top;

            if (card == null)
            {
                HasLost = true;
                return;
            }

            _hand.Add(card);

            Publish(new PlayerDrawsCardEvent(this));

            if (Ai.IsSearchInProgress && !card.IsPeeked)
            {
                card.Hide();
            }
        }

        public void DrawCards(int cardCount)
        {
            for (var i = 0; i < cardCount; i++)
            {
                DrawCard();
            }
        }

        public void DrawStartingHand()
        {
            DrawCards(5);
        }

        public void EmptyManaPool()
        {
            ManaCache.EmptyManaPool();
        }

        public void GetTargets(Func<Zone, Player, bool> zoneFilter, List<ITarget> targets)
        {
            targets.Add(this);
            _battlefield.GenerateZoneTargets(zoneFilter, targets);
            _hand.GenerateZoneTargets(zoneFilter, targets);
            _breakZone.GenerateZoneTargets(zoneFilter, targets);
            _library.GenerateZoneTargets(zoneFilter, targets);
        }

        public bool HasMana(
            int amount,
            ManaUsage usage = ManaUsage.Any,
            ConvokeAndDelveOptions convokeAndDelveOptions = null
        )
        {
            return HasMana(amount.Colorless(), usage, convokeAndDelveOptions);
        }

        public bool HasMana(
            ManaAmount amount,
            ManaUsage usage = ManaUsage.Any,
            ConvokeAndDelveOptions convokeAndDelveOptions = null
        )
        {
            return ManaCache.Has(amount, usage, convokeAndDelveOptions);
        }

        public void PutCardToBreakZone(Card card)
        {
            if (card.IsLimitBreak)
            {
                PutCardToLBRevealed(card);
                return;
            }

            _breakZone.AddToEnd(card);
        }

        public void PutCardToDamageZone(Card card)
        {
            _damageZone.AddToEnd(card);
        }

        public void RemoveDamageFromPermanents()
        {
            foreach (var card in _battlefield)
            {
                card.ClearDamage();
            }
        }

        public void RemoveRegenerationFromPermanents()
        {
            foreach (var permanent in _battlefield)
            {
                permanent.HasRegenerationShield = false;
            }
        }

        public void ShuffleIntoMainDeck(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
                PutOnBottomOfMainDeck(card);
            }

            _library.Shuffle();
        }

        public void ShuffleIntoMainDeck(Card card)
        {
            PutOnBottomOfMainDeck(card);
            _library.Shuffle();
        }

        public void ShuffleMainDeck()
        {
            _library.Shuffle();
        }

        public void TakeMulligan()
        {
            if (!CanMulligan)
                return;

            var mulliganSize = _hand.Count;

            foreach (var card in Hand.ToList())
            {
                _library.PutOnBottom(card);
            }

            for (var i = 0; i < mulliganSize; i++)
            {
                DrawCard();
            }

            HasAlreadyMulliganed = true;
            Publish(new PlayerTookMulliganEvent(this));
        }

        public void PutCardToRemovedFromPlay(Card card)
        {
            _removedFromPlay.Add(card);
        }

        public void PutCardToLBRevealed(Card card)
        {
            card.Reveal();
            _limitBreak.Add(card);
        }

        public void Mill(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var card = _library.Top;

                if (card == null)
                    return;

                if (Ai.IsSearchInProgress)
                {
                    card.Hide();
                }

                _breakZone.AddToEnd(card);
            }
        }

        public void PutCardToHand(Card card)
        {
            if (card.IsLimitBreak)
            {
                PutCardToLBRevealed(card);
                return;
            }

            _hand.Add(card);
        }

        public void PutCardOnTopOfMainDeck(Card card)
        {
            if (card.IsLimitBreak)
            {
                PutCardToLBRevealed(card);
                return;
            }

            _library.PutOnTop(card);
        }

        public void PutCardIntoMainDeckAtPosition(int positionFromTop, Card card)
        {
            if (card.IsLimitBreak)
            {
                PutCardToLBRevealed(card);
                return;
            }

            _library.InsertAt(positionFromTop, card);
        }

        public void PutOnBottomOfMainDeck(Card card)
        {
            if (card.IsLimitBreak)
            {
                PutCardToLBRevealed(card);
                return;
            }

            _library.PutOnBottom(card);
        }

        public void AddToLimitBreakZone(Card card)
        {
            _limitBreak.Add(card);
        }

        public override string ToString()
        {
            return Name;
        }

        public void RevealHand()
        {
            foreach (var card in _hand)
            {
                card.Reveal();
            }
        }

        public void PeekMainDeck()
        {
            foreach (var card in _library)
            {
                card.Peek();
            }

            Publish(new PlayerSearchesMainDeck(this));
        }

        public void ReorderTopCardsOfMainDeck(int[] permutation)
        {
            _library.ReorderFront(permutation);
        }

        public bool ShouldSkipStep(Step step)
        {
            return _skipSteps.Contains(step);
        }

        public void AddEmblem(Emblem emblem)
        {
            _emblems.Add(emblem);
            Publish(new EmblemAddedEvent(emblem));
        }

        public void RemoveEmblem(Emblem emblem)
        {
            _emblems.Remove(emblem);
            Publish(new EmblemRemovedEvent(emblem));
        }

        public bool Controls(Func<IEnumerable<Card>, bool> predicate)
        {
            return predicate(_battlefield.ToArray());
        }

        public int ControlledBackupCount()
        {
            return _battlefield.Count(x => x.Is().Backup);
        }

        public bool ControlsAny(Func<Card, bool> func)
        {
            return Controls(cards => cards.Any(func));
        }
    }
}
