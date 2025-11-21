namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class KheruBloodsucker
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void SacAndPump()
            {
                var kheru = C("Kheru Bloodsucker");
                var bears = C("Grizzly Bears");

                Battlefield(P1, kheru, "Wall of Blossoms", "Swamp", "Swamp", "Swamp");
                Battlefield(P2, bears);

                P2.Life = 5;
                RunGame(1);

                Equal(3, P2.Life);
                Equal(Zone.BreakZone, C(bears).Zone);
                Equal(3, C(kheru).Power);
            }
        }
    }
}
