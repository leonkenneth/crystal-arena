namespace CrystalArena.AI.TargetingRules
{
    using System.Collections.Generic;
    using System.Linq;

    public class EffectTapBackup : TargetingRule
    {
        protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
        {
            var candidates = p.Candidates<Card>(ControlledBy.Opponent)
                .Where(x => !x.IsTapped)
                .OrderByDescending(x => x.Score);

            return Group(candidates, p.TotalMinTargetCount());
        }
    }
}
