namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class RumblingCrescendo
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Destroy3Backups()
            {
                Battlefield(P1, "Rumbling Crescendo", "Mountain");
                Battlefield(P2, "Island", "Island", "Mountain", "Mountain", "Grizzly Bears");

                RunGame(5);

                Equal(1, P2.Battlefield.Backups.Count());
            }
        }
    }
}
