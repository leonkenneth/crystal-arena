namespace CrystalArena.AI.TargetingRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EffectBigWithoutEvasions : TargetingRule
    {
        private readonly Func<Card, bool> _filter;

        private EffectBigWithoutEvasions() { }

        public EffectBigWithoutEvasions(Func<Card, bool> filter = null)
        {
            _filter =
                filter
                ?? delegate
                {
                    return true;
                };
        }

        protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
        {
            var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
                .Where(attacker => _filter(attacker))
                .OrderByDescending(CalculateScore);

            return Group(candidates, p.TotalMinTargetCount());
        }

        private int CalculateScore(Card forward)
        {
            if (!forward.CanAttack)
                return -1;

            return Combat.CouldBeBlockedByAny(forward)
                ? 1
                : forward.CalculateCombatDamageAmount(toPlayer: true, singleDamageStep: false);
        }
    }
}
