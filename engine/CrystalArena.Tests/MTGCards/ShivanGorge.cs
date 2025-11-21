namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class ShivanGorge
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Deal1DamageToOpponent()
            {
                Battlefield(P1, "Shivan Gorge", "Mountain", "Forest", "Forest");
                P2.Life = 1;

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
