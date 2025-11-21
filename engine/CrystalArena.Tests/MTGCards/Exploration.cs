namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Exploration
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Play1Backup()
            {
                Hand(P1, "Forest", "Forest", "Forest");
                RunGame(1);
                Equal(1, P1.Battlefield.Backups.Count());
            }

            [Fact(Skip = "Old card")]
            public void Play2Backups()
            {
                Hand(P1, "Forest", "Forest", "Forest", "Exploration");
                RunGame(1);
                Equal(2, P1.Battlefield.Backups.Count());
            }
        }
    }
}
