namespace CrystalArena.AI.TimingRules
{
    using System.Linq;

    public class NonTargetRemovalTimingRule : TimingRule
    {
        private readonly int _count;

        private NonTargetRemovalTimingRule() { }

        public NonTargetRemovalTimingRule(int count)
        {
            _count = count;
        }

        public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
        {
            return p.Card.IsPermanent || p.Card.Is().Summon ? Summon(p) : Sorcery(p);
        }

        private bool Summon(TimingRuleParameters p)
        {
            var opponentForwardCount = p.Controller.Opponent.Battlefield.Forwards.Count();

            if (opponentForwardCount == 0)
                return false;

            if (opponentForwardCount == 1)
            {
                return (
                    IsBeforeYouDeclareBlockers(p.Controller)
                    || IsBeforeYouDeclareAttackers(p.Controller)
                );
            }

            if (opponentForwardCount > 2 * _count + 1)
                return false;

            return IsEndOfOpponentsTurn(p.Controller);
        }

        private bool Sorcery(TimingRuleParameters p)
        {
            var opponentForwardCount = p.Controller.Opponent.Battlefield.Forwards.Count();

            if (opponentForwardCount == 0)
                return false;

            if (opponentForwardCount == 1)
                return Turn.Step == Step.FirstMain;

            if (opponentForwardCount > 2 * _count + 1)
                return false;

            return Turn.Step == Step.SecondMain;
        }
    }
}
