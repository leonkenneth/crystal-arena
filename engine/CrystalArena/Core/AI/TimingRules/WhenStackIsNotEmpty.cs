namespace CrystalArena.AI.TimingRules
{
    public class WhenStackIsNotEmpty : TimingRule
    {
        public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
        {
            return !Stack.IsEmpty;
        }
    }
}
