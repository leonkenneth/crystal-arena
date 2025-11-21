namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Backupslide
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Deal5DamageToOpponent()
            {
                Hand(P1, "Backupslide");
                Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
                P2.Life = 0;

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
