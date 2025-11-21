namespace CrystalArena.Modifiers
{
    using System.Collections.Generic;

    public class ChangeBasicBackupSubtype : Modifier, ICardModifier
    {
        private readonly string _backupSubtype;
        private readonly bool _replace;
        private ActivatedAbilities _abilities;
        private ActivatedAbility _addedAbility;
        private TypeOfCard _typeOfCard;
        private CardTypeSetter _cardTypeModifier;
        private PropertyModifier<List<ActivatedAbility>> _modifier;

        private static Dictionary<string, string> BasicBackupTypeToManaSymbol = new Dictionary<
            string,
            string
        >()
        {
            { "swamp", "{B}" },
            { "island", "{U}" },
            { "mountain", "{R}" },
            { "forest", "{G}" },
            { "plains", "{W}" },
        };

        private ChangeBasicBackupSubtype() { }

        public ChangeBasicBackupSubtype(string backupSubtype, bool replace)
        {
            _backupSubtype = backupSubtype.ToLowerInvariant();
            _replace = replace;
        }

        public override void Apply(ActivatedAbilities abilities)
        {
            _abilities = abilities;

            var ap = new ManaAbilityParameters
            {
                Text = string.Format(
                    "{{T}}: Add {0} to your mana pool.",
                    BasicBackupTypeToManaSymbol[_backupSubtype]
                ),
            };

            ap.ManaAmount(Mana.GetBasicBackupMana(_backupSubtype));
            _addedAbility = new ManaAbility(ap);
            _addedAbility.Initialize(OwningCard, Game);

            if (_replace)
            {
                _modifier = new SetList<ActivatedAbility>(
                    new List<ActivatedAbility> { _addedAbility }
                );
            }
            else
            {
                _modifier = new AddToList<ActivatedAbility>(_addedAbility);
            }

            _modifier.Initialize(ChangeTracker);
            _abilities.AddModifier(_modifier);
        }

        public override void Apply(TypeOfCard typeOfCard)
        {
            _typeOfCard = typeOfCard;

            var type = _typeOfCard.Value.Change(subTypes: _backupSubtype);
            _cardTypeModifier = new CardTypeSetter(type);
            _cardTypeModifier.Initialize(ChangeTracker);

            _typeOfCard.AddModifier(_cardTypeModifier);
        }

        protected override void Unapply()
        {
            _typeOfCard.RemoveModifier(_cardTypeModifier);
            _abilities.RemoveModifier(_modifier);
        }
    }
}
