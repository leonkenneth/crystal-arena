namespace CrystalArena
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;

    public interface ITargetType
    {
        bool Artifact { get; }
        bool Attachment { get; }
        bool BasicBackup { get; }
        bool Forward { get; }
        bool Monster { get; }
        bool Equipment { get; }
        bool Summon { get; }
        bool Backup { get; }
        bool Legendary { get; }
        bool Sorcery { get; }
        bool Token { get; }
        bool Aura { get; }
        bool NonBasicBackup { get; }
        bool Planeswalker { get; }

        bool Character
        {
            get { return Backup || Monster || Forward; }
        }

        bool Party { get; }
    }

    public class CardType : IHashable, ITargetType
    {
        private static readonly HashSet<string> BasicTypes = new HashSet<string>(
            new[]
            {
                "artifact",
                "backup",
                "monster",
                "summon",
                "backup",
                "plane",
                "planeswalker",
                "scheme",
                "sorcery",
                "tribal",
                "vanguard",
                "token",
                "forward",
            }
        );

        private readonly HashSet<string> _baseTypes = new HashSet<string>();
        private readonly HashSet<string> _subTypes = new HashSet<string>();
        private readonly HashSet<string> _superTypes = new HashSet<string>();

        private string _display;

        private bool _isArtifact;
        private bool _isAura;
        private bool _isBasicBackup;
        private bool _isForward;
        private bool _isMonster;
        private bool _isEquipment;
        private bool _isSummon;
        private bool _isBackup;
        private bool _isLegendary;
        private bool _isSorcery;
        private bool _isToken;
        private bool _isPlaneswalker;

        private CardType(
            HashSet<string> superTypes,
            HashSet<string> baseTypes,
            HashSet<string> subTypes
        )
        {
            _superTypes = superTypes;
            _baseTypes = baseTypes;
            _subTypes = subTypes;

            Init();
        }

        public CardType(string typeString)
        {
            var types = ParseTypeString(typeString);
            var active = _superTypes;

            foreach (var type in types)
            {
                if (BasicTypes.Contains(type))
                {
                    active = _subTypes;
                    _baseTypes.Add(type);
                }
                else
                {
                    active.Add(type);
                }
            }

            Init();
        }

        public IEnumerable<string> BaseTypes
        {
            get { return _baseTypes; }
        }

        public IEnumerable<string> SubTypes
        {
            get { return _subTypes; }
        }

        public IEnumerable<string> SuperTypes
        {
            get { return _superTypes; }
        }

        public static CardType None
        {
            get { return new CardType(String.Empty); }
        }

        public bool Artifact
        {
            get { return _isArtifact; }
        }

        public bool Attachment
        {
            get { return Aura || Equipment; }
        }

        public bool BasicBackup
        {
            get { return _isBasicBackup; }
        }

        public bool Forward
        {
            get { return _isForward; }
        }

        public bool Monster
        {
            get { return _isMonster; }
        }

        public bool Equipment
        {
            get { return _isEquipment; }
        }

        public bool Summon
        {
            get { return _isSummon; }
        }

        public bool Backup
        {
            get { return _isBackup; }
        }

        public bool Legendary
        {
            get { return _isLegendary; }
        }

        public bool Sorcery
        {
            get { return _isSorcery; }
        }

        public bool Token
        {
            get { return _isToken; }
        }

        public bool Aura
        {
            get { return _isAura; }
        }

        public bool NonBasicBackup
        {
            get { return Backup && !BasicBackup; }
        }

        public bool Planeswalker
        {
            get { return _isPlaneswalker; }
        }

        public bool Party => false;

        private void Init()
        {
            var superAndMain = String.Join(
                " ",
                _superTypes
                    .OrderBy(x => x)
                    .Concat(_baseTypes.OrderBy(x => x))
                    .Select(x => x.Capitalize())
            );

            if (_subTypes.Count > 0)
            {
                var sub = String.Join(" ", _subTypes.OrderBy(x => x).Select(x => x.Capitalize()));

                _display = String.Format("{0} — {1}", superAndMain, sub);
            }
            else
            {
                _display = superAndMain;
            }

            _isForward = Is("forward");
            _isBackup = Is("backup");
            _isBasicBackup = Is("basic backup");
            _isLegendary = Is("legendary");
            _isArtifact = Is("artifact");
            _isMonster = Is("monster");
            _isEquipment = Is("equipment");
            _isAura = Is("aura");
            _isSummon = Is("summon");
            _isSorcery = Is("sorcery");
            _isToken = Is("token");
            _isPlaneswalker = Is("planeswalker");
        }

        private static string[] ParseTypeString(string typeString)
        {
            return typeString
                .Split(new[] { ' ', '-', '—' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLowerInvariant())
                .ToArray();
        }

        public bool Is(string typeString)
        {
            var types = ParseTypeString(typeString);

            foreach (var type in types)
            {
                if (_superTypes.Contains(type))
                    continue;

                if (_baseTypes.Contains(type))
                    continue;

                if (_subTypes.Contains(type))
                    continue;

                return false;
            }

            return true;
        }

        public bool IsAny(params string[] types)
        {
            return IsAny(types.AsEnumerable());
        }

        public bool IsAny(IEnumerable<string> types)
        {
            return types.Any(Is);
        }

        public override string ToString()
        {
            return _display;
        }

        public int CalculateHash(HashCalculator calc)
        {
            return _display.GetHashCode();
        }

        public static implicit operator CardType(string cardTypes)
        {
            return new CardType(cardTypes);
        }

        public CardType Change(
            string superTypes = null,
            string baseTypes = null,
            string subTypes = null
        )
        {
            var super =
                superTypes == null
                    ? new HashSet<string>(_superTypes)
                    : new HashSet<string>(ParseTypeString(superTypes));

            var baseT =
                baseTypes == null
                    ? new HashSet<string>(_baseTypes)
                    : new HashSet<string>(ParseTypeString(baseTypes));

            var sub =
                subTypes == null
                    ? new HashSet<string>(_subTypes)
                    : new HashSet<string>(ParseTypeString(subTypes));

            return new CardType(super, baseT, sub);
        }

        public CardType Add(
            string superTypes = null,
            string baseTypes = null,
            string subTypes = null
        )
        {
            var super = new HashSet<string>(_superTypes);
            var sub = new HashSet<string>(_subTypes);
            var baseT = new HashSet<string>(_baseTypes);

            if (superTypes != null)
            {
                super.UnionWith(ParseTypeString(superTypes));
            }

            if (baseTypes != null)
            {
                baseT.UnionWith(ParseTypeString(baseTypes));
            }

            if (subTypes != null)
            {
                sub.UnionWith(ParseTypeString(subTypes));
            }

            return new CardType(super, baseT, sub);
        }
    }
}
