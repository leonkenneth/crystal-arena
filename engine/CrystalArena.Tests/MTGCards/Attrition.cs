namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Attrition
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void SacRatsToKillDragon()
            {
                Battlefield(P1, "Ravenous Rats", "Shivan Dragon", "Swamp", "Mountain", "Attrition");
                Battlefield(P2, "Shivan Dragon");

                P2.Life = 6;

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
