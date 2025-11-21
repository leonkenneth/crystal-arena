namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class CrucibleOfFire
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void DragonHasIncreasedStrength()
            {
                Battlefield(P1, "Crucible Of Fire", "Shivan Dragon");

                RunGame(1);

                Equal(12, P2.Life);
            }
        }
    }
}
