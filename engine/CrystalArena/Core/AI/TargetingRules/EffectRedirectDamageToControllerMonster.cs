namespace CrystalArena.AI.TargetingRules
{
    using System.Collections.Generic;
    using System.Linq;

    public class EffectRedirectDamageToControllerMonster : TargetingRule
    {
        protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
        {
            var candidates = p.Candidates<Card>(ControlledBy.Opponent)
                .OrderByDescending(x => x.Toughness);

            return Group(candidates, 1);
        }
    }
}
