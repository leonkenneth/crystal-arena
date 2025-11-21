namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Opposition
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void TapOpponentForwardsAndAttack()
            {
                Battlefield(P1, "Fog Bank", "Fog Bank");
                P1.Life = 6;

                Battlefield(
                    P2,
                    "Llanowar Elves",
                    "Llanowar Elves",
                    "Wurmcoil Engine",
                    "Opposition"
                );

                RunGame(2);

                Equal(-2, P1.Life);
            }
        }
    }
}
