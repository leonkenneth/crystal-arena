namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class GlacialCrasher
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void CantAttack()
            {
                Battlefield(P1, "Glacial Crasher");
                P2.Life = 5;

                RunGame(1);
                Equal(5, P2.Life);
            }

            [Fact(Skip = "Old card")]
            public void CanAttack()
            {
                Hand(P1, "Mountain");
                Battlefield(P1, "Glacial Crasher");
                P2.Life = 5;

                RunGame(1);
                Equal(0, P2.Life);
            }
        }
    }
}
