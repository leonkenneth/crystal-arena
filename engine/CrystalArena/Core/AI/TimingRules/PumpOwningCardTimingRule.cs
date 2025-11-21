namespace CrystalArena.AI.TimingRules
{
    using Modifiers;

    public class PumpOwningCardTimingRule : TimingRule
    {
        private readonly Value _power;
        private readonly Value _toughness;

        private PumpOwningCardTimingRule() { }

        public PumpOwningCardTimingRule(Value power, Value toughness)
        {
            _power = power;
            _toughness = toughness;
        }

        public override bool ShouldPlayAfterTargets(TimingRuleParameters p)
        {
            var power = _power.GetValue(p.X);
            var toughness = _toughness.GetValue(p.X);

            if (toughness > 0 && Stack.CanBeDealtLeathalDamageByTopSpell(p.Card))
            {
                return true;
            }

            if (IsAfterOpponentDeclaresBlocker(p.Controller) && p.Card.IsAttacker)
            {
                return QuickCombat.CalculateGainAttackerWouldGetIfPowerAndThoughnessWouldIncrease(
                        attacker: p.Card,
                        blocker: Combat.Blocker?.Card,
                        powerIncrease: power,
                        toughnessIncrease: toughness
                    ) > 0;
            }

            if (IsAfterYouDeclareBlockers(p.Controller) && p.Card.IsBlocker)
            {
                return QuickCombat.CalculateGainBlockerWouldGetIfPowerAndThougnessWouldIncrease(
                        blocker: p.Card,
                        party: new Party(Combat.Attackers),
                        powerIncrease: power,
                        toughnessIncrease: toughness
                    ) > 0;
            }

            return false;
        }
    }
}
