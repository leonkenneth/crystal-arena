namespace CrystalArena.AI
{
    using System.Collections.Generic;
    using System.Linq;
    using Decisions;
    using Infrastructure;

    public class BlockStrategy
    {
        public static ChosenBlocker ChooseBlocker(BlockStrategyParameters p)
        {
            var candidates = new List<BlockerCandidate>();
            var party = new Party(p.Attackers);

            foreach (var blocker in p.BlockerCandidates)
            {
                if (!party.CanBeBlockedBy(blocker))
                    continue;

                candidates.Add(new BlockerCandidate(party, blocker, p.DefendersLife));
            }

            var chosenCandidate = candidates
                .Where(x => x.Gain > 0)
                .OrderByDescending(x => x.Gain)
                .FirstOrDefault();

            if (chosenCandidate != null)
            {
                return new ChosenBlocker { Blocker = chosenCandidate.Blocker };
            }

            return ChosenBlocker.None;
        }

        private class BlockerCandidate
        {
            public BlockerCandidate(Party party, Card blocker, int defenderLife)
            {
                Party = party;
                Blocker = blocker;
                ComputeGain(party, blocker, defenderLife);
            }

            public Party Party { get; private set; }

            public Card Blocker { get; private set; }

            public int Gain { get; private set; }

            private void ComputeGain(Party party, Card blocker, int defendersLife)
            {
                var canBlockerBeDealtLeathalCombatDamage =
                    QuickCombat.CanBlockerBeDealtLeathalCombatDamage(party, blocker);

                var blockerScore = canBlockerBeDealtLeathalCombatDamage ? blocker.Score : 0;

                var lossIfNotBlocked = QuickCombat.CalculateDefendingPlayerLifeloss(
                    new CombatEvaluationParameters(party.Attackers, null)
                );
                var lifelossScore = ScoreCalculator.CalculateLifelossScore(
                    defendersLife,
                    lossIfNotBlocked.Total
                );

                Gain = lifelossScore - blockerScore;

                var attackerScore = party.Attackers.Sum(attacker =>
                {
                    var isAttackerKilled = QuickCombat.CanAttackerBeDealtLeathalDamage(
                        attacker,
                        blocker
                    );
                    return isAttackerKilled ? attacker.Score : 0;
                });

                Gain += attackerScore;
            }
        }
    }
}
