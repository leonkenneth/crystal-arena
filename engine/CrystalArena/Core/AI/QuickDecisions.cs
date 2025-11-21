namespace CrystalArena.AI
{
    using System.Collections.Generic;
    using System.Linq;
    using Decisions;

    public static class QuickDecisions
    {
        public static Ordering OrderTopCards(List<Card> candidates, Player controller)
        {
            var backupsInPlay = controller.Battlefield.Backups.Count();

            var needsBackups = !controller.Hand.Backups.Any() && backupsInPlay <= 5;

            var indices = Enumerable.Repeat(0, candidates.Count).ToArray();

            var ordering = candidates
                .Select(
                    (x, i) =>
                    {
                        int score;

                        if (x.Is().Backup)
                        {
                            score = needsBackups ? 100 : 0;
                        }
                        else if (x.ConvertedCost <= backupsInPlay)
                        {
                            score = x.ConvertedCost;
                        }
                        else
                        {
                            score = -x.ConvertedCost;
                        }

                        return new
                        {
                            Card = x,
                            Index = i,
                            Score = score,
                        };
                    }
                )
                .OrderByDescending(x => x.Score)
                .Select(x => x.Index)
                .ToList();

            for (var i = 0; i < ordering.Count; i++)
            {
                indices[ordering[i]] = i;
            }

            return new Ordering(indices);
        }
    }
}
