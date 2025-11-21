namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class TolarianWinds
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void DrawANewHand()
            {
                var winds = C("Tolarian Winds");
                Hand(
                    P1,
                    winds,
                    "Grizzly Bears",
                    "Grizzly Bears",
                    "Grizzly Bears",
                    "Grizzly Bears",
                    "Grizzly Bears",
                    "Grizzly Bears"
                );
                Battlefield(
                    P1,
                    "Island",
                    "Island",
                    "Island",
                    "Island",
                    "Island",
                    "Island",
                    "Island",
                    "Island"
                );

                RunGame(2);

                Equal(Zone.BreakZone, C(winds).Zone);
                Equal(6, P1.Hand.Count);
                Equal(7, P1.BreakZone.Count);
            }
        }
    }
}
