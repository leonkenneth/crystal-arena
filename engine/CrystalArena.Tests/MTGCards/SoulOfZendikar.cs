namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class SoulOfZendikar
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void CreateToken()
            {
                Battlefield(
                    P1,
                    "Soul of Zendikar",
                    "Forest",
                    "Forest",
                    "Forest",
                    "Forest",
                    "Forest",
                    "Forest"
                );

                RunGame(2);

                Equal(1, P1.Battlefield.Count(c => c.Is().Token));
            }
        }
    }
}
