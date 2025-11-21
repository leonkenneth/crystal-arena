namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class RainOfFilth
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Cast2Specters()
            {
                Hand(P1, "Hypnotic Specter", "Hypnotic Specter", "Rain of Filth");
                Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp");
                P2.Life = 4;

                RunGame(3);
                Equal(0, P2.Life);
            }
        }
    }
}
