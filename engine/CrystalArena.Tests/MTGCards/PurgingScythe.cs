namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class PurgingScythe
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void KillSomnophore()
            {
                var somnophore = C("Somnophore");

                Battlefield(P1, "Grizzly Bears", "Purging Scythe");
                Battlefield(P2, somnophore, "Grizzly Bears");

                RunGame(1);

                Equal(Zone.BreakZone, C(somnophore).Zone);
            }
        }
    }
}
