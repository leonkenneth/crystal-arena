namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class FirstResponse
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Player2GetsAToken()
            {
                Battlefield(P1, "Shivan Dragon");
                Battlefield(P2, "First Response");

                RunGame(2);

                Equal(15, P2.Life);
                Equal(1, P2.Battlefield.Count(x => x.Is().Token));
            }
        }
    }
}
