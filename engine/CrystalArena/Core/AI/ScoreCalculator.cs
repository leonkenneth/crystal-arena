namespace CrystalArena.AI
{
    using System;
    using System.Linq;

    public static class ScoreCalculator
    {
        public static readonly int HiddenCardInHandScore = 220;

        public static int CalculateTapPenalty(Card card, TurnInfo turnInfo)
        {
            if (card.Is().Backup)
            {
                if (card.Controller.IsActive)
                {
                    if (turnInfo.Step == Step.Upkeep)
                        return 10;

                    return 2;
                }
            }

            return 1;
        }

        public static int CalculateDiscardScore(Card card, bool isSearchInProgress)
        {
            if (isSearchInProgress && card.IsVisibleToSearchingPlayer == false)
            {
                return HiddenCardInHandScore;
            }

            // the lower the score, the more likely card will be discarded
            var hand = card.Controller.Hand;
            var battlefield = card.Controller.Battlefield;

            if (card.Is().Backup)
            {
                var backupHandCount = hand.Count(x => x.Is().Backup);
                var backupBattlefieldCount = battlefield.Count(x => x.Is().Backup);

                if ((backupHandCount + backupBattlefieldCount) < 4)
                    return int.MaxValue;

                if (backupHandCount < 2)
                    return int.MaxValue;

                return 0 - 2 * backupBattlefieldCount;
            }

            return card.ConvertedCost;
        }

        public static int CalculateLifeScore(int life)
        {
            var score = 5000;

            if (life > Life.MaxLife)
                return score + (life - Life.MaxLife) * 40;

            if (life <= 0)
            {
                return -1000 + 500 * life;
            }

            return score + Scores.LifeToScore[life];
        }

        public static int CalculatePermanentScore(Card permanent)
        {
            var score = 0;

            if (permanent.OverrideScore.Battlefield.HasValue)
                return permanent.OverrideScore.Battlefield.Value;

            if (permanent.Level > 0)
                score += 10 * permanent.Level.Value;

            if (permanent.ManaCost != null)
            {
                score += CalculatePermanentScoreFromManaCost(permanent);

                if (permanent.Is().Forward)
                {
                    score += (
                        permanent.Power.Value / 2000 * 10 + permanent.Toughness.Value / 2000 * 3
                    );

                    if (permanent.HasSummoningSickness)
                        score -= 1;
                }
            }
            else if (permanent.Is().Forward)
            {
                score += CalculatePermanentScoreFromPowerToughness(
                    permanent.Power.Value,
                    permanent.Toughness.Value
                );
            }
            else if (permanent.Is().Backup)
            {
                score += GetBackupOnBattlefieldScore(permanent);
                if (!permanent.Is().BasicBackup)
                    score += 10;
            }

            if (permanent.CountersCount() > 0)
            {
                score += permanent.CountersCount() * 10;
            }

            return score;
        }

        private static int GetBackupOnBattlefieldScore(Card backup)
        {
            var backupCount = backup.Controller.Battlefield.Backups.Count();
            return backupCount > 6 ? 350 : Scores.BackupsOnBattlefieldToBackupScore[backupCount];
        }

        private static int CalculatePermanentScoreFromManaCost(Card permanent)
        {
            var converted = Math.Min(7, permanent.ManaCost.Converted);

            if (permanent.Has().Haste && converted > 0)
            {
                converted--;
            }

            return permanent.Has().Echo
                ? Scores.ManaCostToScoreEcho[converted]
                : Scores.ManaCostToScore[converted];
        }

        private static int CalculateCardInHandScoreFromManaCost(ManaAmount mana)
        {
            var converted = Math.Min(7, mana.Converted);
            var score = Scores.ManaCostToScore[converted] - 100;
            return score > 120 ? score : 120;
        }

        private static int CalculatePermanentScoreFromPowerToughness(int power, int toughness)
        {
            var scoredPower = power / 2000;

            if (scoredPower < 0)
                scoredPower = 0;
            else if (scoredPower > 10)
                scoredPower = 10;

            return Scores.PowerToughnessToScore[scoredPower];
        }

        public static int CalculateCardInHandScore(Card card, bool isSearchInProgress)
        {
            if (isSearchInProgress && card.IsVisibleToSearchingPlayer == false)
            {
                return HiddenCardInHandScore;
            }

            if (card.OverrideScore.Hand.HasValue)
                return card.OverrideScore.Hand.Value;

            if (card.ManaCost == null || card.ManaCost.Converted == 0)
            {
                return Scores.BackupInHandCost;
            }

            return CalculateCardInHandScoreFromManaCost(card.ManaCost);
        }

        public static int CalculateCardInBreakZoneScore(Card card)
        {
            if (card.OverrideScore.BreakZone.HasValue)
                return card.OverrideScore.BreakZone.Value;

            if (card.Is().BasicBackup)
                return 1;

            if (card.Is().Backup)
                return 2;

            // Shouldn't be in breakzone but we remove the conditions
            // to move tokens to exile on leaving field
            if (card.Is().Token)
                return 0;

            return card.ManaCost.Converted;
        }

        public static int CalculateLifelossScore(int life, int loss)
        {
            return CalculateLifeScore(life) - CalculateLifeScore(life - loss);
        }

        public static int CalculateCardInMainDeckScore(Card card)
        {
            if (card.OverrideScore.MainDeck.HasValue)
                return card.OverrideScore.MainDeck.Value;

            return CalculateCardInBreakZoneScore(card) - 1;
        }
    }
}
