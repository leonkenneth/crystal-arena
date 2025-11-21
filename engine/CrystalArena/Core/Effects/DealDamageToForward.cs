namespace CrystalArena.Effects
{
    public class DealDamageToForward : Effect
    {
        private readonly DynParam<int> _amount;
        private readonly DynParam<Card> _forward;

        private DealDamageToForward() { }

        public DealDamageToForward(DynParam<int> amount, DynParam<Card> forward)
        {
            _amount = amount;
            _forward = forward;

            RegisterDynamicParameters(forward, amount);
        }

        public override int CalculateForwardDamage(Card card)
        {
            return card == _forward.Value ? _amount.Value : 0;
        }

        protected override void ResolveEffect()
        {
            Source.OwningCard.DealDamageTo(_amount.Value, _forward.Value, isCombat: false);
        }
    }
}
