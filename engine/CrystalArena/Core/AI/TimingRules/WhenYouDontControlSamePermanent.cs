namespace CrystalArena.AI.TimingRules
{
    using CrystalArena.Infrastructure;

    public class WhenYouDontControlSamePermanent : TimingRule
    {
        public override bool ShouldPlayAfterTargets(TimingRuleParameters p)
        {
            return p.Controller.Battlefield.None(x => x.Name == p.Card.Name);
        }
    }
}
