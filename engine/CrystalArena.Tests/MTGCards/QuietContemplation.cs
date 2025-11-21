namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class QuietContemplation
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void ShockAndTap()
            {
                Hand(P1, "Shock");
                Battlefield(
                    P1,
                    "Grizzly Bears",
                    "Grizzly Bears",
                    "Quiet Contemplation",
                    "Mountain",
                    "Mountain"
                );
                Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

                P2.Life = 8;

                RunGame(3);

                Equal(0, P2.Life);
            }
        }
    }
}
