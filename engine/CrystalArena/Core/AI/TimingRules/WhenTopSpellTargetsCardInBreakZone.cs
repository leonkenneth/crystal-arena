namespace CrystalArena.AI.TimingRules
{
    using System.Linq;

    public class WhenTopSpellTargetsCardInBreakZone : TimingRule
    {
        public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
        {
            return Stack.TopSpell != null
                && Stack.TopSpell.Controller == p.Controller.Opponent
                && Stack.TopSpell.Targets.Effect.Any(c => Target.Zone(c) == Zone.BreakZone);
        }
    }
}
