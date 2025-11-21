namespace CrystalArena.Effects
{
    using System;
    using Modifiers;

    public class PreventDamageToEquipedForward : Effect
    {
        private readonly Func<Card, int> _amount;

        private PreventDamageToEquipedForward() { }

        public PreventDamageToEquipedForward(Func<Card, int> amount)
        {
            _amount = amount;
        }

        protected override void ResolveEffect()
        {
            var mp = new ModifierParameters { SourceCard = Source.OwningCard, SourceEffect = this };

            var prevention = new PreventDamageToTarget(
                target: Source.OwningCard.AttachedTo,
                amount: (forwardOrPlayer, ctx) => _amount((Card)(forwardOrPlayer))
            );

            var modifier = new AddDamagePrevention(prevention);
            Game.AddModifier(modifier, mp);
        }
    }
}
