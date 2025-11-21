namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class GoblinMatron
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void FetchAGoblin()
            {
                var buggy = C("Goblin War Buggy");
                MainDeck(P1, buggy, "Forest", "Forest");
                Hand(P1, "Goblin Matron");
                Battlefield(P1, "Forest", "Mountain", "Mountain");

                RunGame(1);

                Equal(Zone.Hand, C(buggy).Zone);
            }
        }
    }
}
