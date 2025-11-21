namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Sunder
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void BounceBackups()
            {
                Hand(P1, "Sunder");
                Hand(P2, "Swamp", "Swamp", "Swamp", "Swamp");

                Battlefield(P1, "Island", "Island", "Island", "Island", "Island");
                Battlefield(P2, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");

                RunGame(3);

                Equal(1, P1.Battlefield.Backups.Count());
                Equal(0, P2.Battlefield.Backups.Count());
            }
        }
    }
}
