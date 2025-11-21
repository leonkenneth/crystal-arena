namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class PhyrexianBroodlings
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void AttackToKill()
            {
                Battlefield(P1, "Wall of Blossoms", "Phyrexian Broodlings", "Swamp");
                P2.Life = 3;

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
