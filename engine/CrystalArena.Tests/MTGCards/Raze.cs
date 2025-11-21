namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Raze
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void DestroyBackup()
            {
                Hand(P1, "Raze");
                Battlefield(P1, "Mountain", "Mountain", "Island", "Island", "Island", "Island");
                Battlefield(P2, "Mountain", "Forest");

                RunGame(1);

                Equal(1, P2.Battlefield.Backups.Count());
                Equal(5, P1.Battlefield.Backups.Count());
            }
        }
    }
}
