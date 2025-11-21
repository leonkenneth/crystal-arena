namespace CrystalArena.AI.TargetingRules
{
    using System.Collections.Generic;
    using System.Linq;

    public class EffectExchangeBattlefieldBreakZone : TargetingRule
    {
        protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
        {
            var battlefieldCandidates = p.Candidates<Card>(ControlledBy.SpellOwner)
                .OrderBy(x => x.Score);

            var breakZoneCandidates = p.Candidates<Card>(ControlledBy.SpellOwner, selectorIndex: 1)
                .OrderBy(x => -x.Score);

            return Group(battlefieldCandidates, breakZoneCandidates);
        }
    }
}
