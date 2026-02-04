namespace CrystalArena.Effects
{
    public class CounterTriggeredAbilityAndDamageSourceForward : Effect
    {
        private readonly DynParam<int> _damageAmount;

        private CounterTriggeredAbilityAndDamageSourceForward() { }

        public CounterTriggeredAbilityAndDamageSourceForward(int damageAmount)
        {
            _damageAmount = damageAmount;
            RegisterDynamicParameters(_damageAmount);
        }

        protected override void ResolveEffect()
        {
            var targetedEffect = Target.Effect();
            var sourceCard = targetedEffect.Source.OwningCard;

            // Counter the triggered ability
            Stack.Counter(targetedEffect);

            // If the source is a Forward on the battlefield, deal damage
            if (sourceCard.Is().Forward && sourceCard.Zone == Zone.Battlefield)
            {
                Source.OwningCard.DealDamageTo(_damageAmount.Value, sourceCard, isCombat: false);
            }
        }
    }
}
