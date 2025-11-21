namespace CrystalArena.Costs
{
    public class ChangeManaCostOfEnchantedForwardsAbilities : CostModifier
    {
        private ChangeManaCostOfEnchantedForwardsAbilities() { }

        public ChangeManaCostOfEnchantedForwardsAbilities(int amount)
            : base(amount) { }

        protected override bool ShouldApply(Card card, CostType type)
        {
            if (type != CostType.Ability)
                return false;

            return card == Source.AttachedTo;
        }
    }
}
