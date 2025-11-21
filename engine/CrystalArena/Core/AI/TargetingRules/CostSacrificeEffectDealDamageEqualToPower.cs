namespace CrystalArena.AI.TargetingRules
{
    using System.Collections.Generic;
    using System.Linq;

    public class CostSacrificeEffectDealDamageEqualToPower : TargetingRule
    {
        protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
        {
            var costCandidates = p.Candidates<Card>(selectorIndex: 0, selector: c => c.Cost);

            var canTargetOpponent = p.Candidates<Player>(selectorIndex: 0, selector: c => c.Effect)
                .Any(x => x != p.Controller);

            // check to see if we can kill opponent by sacing any one forward
            var opponentsLife = p.Controller.Opponent.Life;
            if (canTargetOpponent && costCandidates.Any(x => opponentsLife <= x.Power))
            {
                var targets = new Targets(
                    cost: costCandidates
                        .Where(x => opponentsLife <= x.Power)
                        .OrderBy(x => x.Score)
                        .First(),
                    effect: p.Controller.Opponent
                );

                return new[] { targets };
            }

            // pair your forwards and opponents
            // power -> lifepoints left

            var yourForwards = costCandidates.OrderBy(x => x.Power).ThenBy(x => x.Score).ToList();

            var opponentForwards = p.Candidates<Card>(selectorIndex: 0, selector: c => c.Effect)
                .OrderBy(x => x.Life)
                .ToList();

            var pairs = (
                from yourForward in yourForwards
                from opponentForward in opponentForwards
                where yourForward.Power >= opponentForward.Life
                select new Pair(yourForward, opponentForward)
            )
                .OrderByDescending(x => x.Score)
                .ToList();

            return pairs.Select(x => new Targets(cost: x.Yours, effect: x.Opponents));
        }

        private class Pair
        {
            public readonly Card Opponents;
            public readonly Card Yours;

            public Pair(Card yours, Card opponents)
            {
                Yours = yours;
                Opponents = opponents;
            }

            public int Score
            {
                get { return Opponents.Score - Yours.Score; }
            }
        }
    }
}
