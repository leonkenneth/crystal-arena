namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class ThranWarMachine
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void ThranAttacksAndIsKilled()
            {
                var thran = C("Thran War Machine");
                Battlefield(P1, thran, "Forest", "Forest", "Forest", "Forest");
                Battlefield(P2, "Verdant Force");

                RunGame(1);

                Equal(Zone.BreakZone, C(thran).Zone);
            }
        }
    }
}
