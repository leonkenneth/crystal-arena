namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class SelflessCathar
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PumpForwardsToDealFinalBlow()
            {
                Battlefield(
                    P1,
                    "Selfless Cathar",
                    "Grizzly Bears",
                    "Grizzly Bears",
                    "Grizzly Bears",
                    "Grizzly Bears",
                    "Grizzly Bears",
                    "Plains",
                    "Plains"
                );

                P2.Life = 15;

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
