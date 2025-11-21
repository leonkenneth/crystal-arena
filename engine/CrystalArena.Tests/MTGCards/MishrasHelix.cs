namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class MishrasHelix
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void TapOpponentBackups()
            {
                Hand(P1, "Grizzly Bears");
                Battlefield(P1, "Forest", "Forest");
                Battlefield(P2, "Grizzly Bears", "Mishra's helix", "Forest", "Forest");

                RunGame(2);

                Equal(2, P1.Battlefield.Count);
                Equal(18, P1.Life);
            }
        }
    }
}
