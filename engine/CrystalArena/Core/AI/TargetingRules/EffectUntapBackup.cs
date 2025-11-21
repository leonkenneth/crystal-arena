namespace CrystalArena.AI.TargetingRules
{
    using System.Collections.Generic;
    using System.Linq;

    public class EffectUntapBackup : TargetingRule
    {
        protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
        {
            var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
                .OrderByDescending(x => x.IsTapped ? 1 : 0)
                .Select(x => x)
                .ToList();

            return Group(candidates, p.TotalMinTargetCount());
        }
    }
}
