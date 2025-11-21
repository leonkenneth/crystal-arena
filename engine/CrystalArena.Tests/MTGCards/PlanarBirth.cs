namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class PlanarBirth
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void ReturnBackups()
            {
                Hand(P1, "Planar Birth");

                Battlefield(P1, "Plains", "Forest");
                Battlefield(P2, "Swamp");
                BreakZone(P1, "Forest", "Forest", "Forest");
                BreakZone(P2, "Swamp");

                RunGame(1);

                Equal(5, P1.Battlefield.Backups.Count());
                Equal(2, P2.Battlefield.Backups.Count());
            }
        }
    }
}
