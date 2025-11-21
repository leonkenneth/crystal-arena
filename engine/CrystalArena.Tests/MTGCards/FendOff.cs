namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class FendOff
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void KillBlocker()
            {
                var roach = C("Giant Cockroach");
                var armoddon = C("Trained Armodon");

                Hand(P1, "Fend off");
                Battlefield(P1, roach, "Plains", "Swamp");

                Battlefield(P2, armoddon);

                RunGame(1);

                Equal(Zone.Battlefield, C(roach).Zone);
                Equal(Zone.BreakZone, C(armoddon).Zone);
            }
        }
    }
}
