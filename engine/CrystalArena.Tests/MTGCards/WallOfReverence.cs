namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class WallOfReverence
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void GainLifeEqualToForwardWithGreatestPower()
            {
                var wall = C("Wall of Reverence");
                Battlefield(P1, wall, "Grizzly Bears");
                RunGame(1);

                Equal(P1.Life, 22);
            }
        }
    }
}
