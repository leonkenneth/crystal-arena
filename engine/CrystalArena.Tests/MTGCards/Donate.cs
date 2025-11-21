namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Donate
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void DonateDelusions()
            {
                Hand(P1, "Donate", "Disenchant");
                Battlefield(
                    P1,
                    "Delusions of Mediocrity",
                    "Plains",
                    "Island",
                    "Island",
                    "Plains",
                    "Plains"
                );
                P2.Life = 10;

                RunGame(2);

                Equal(0, P2.Life);
            }
        }
    }
}
