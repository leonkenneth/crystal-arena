namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class RuptureSpire
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Pay1()
            {
                var backup = C("Mountain");
                var spire = C("Rupture Spire");

                Hand(P1, spire);
                Battlefield(P1, backup);

                RunGame(1);

                Equal(Zone.Battlefield, C(spire).Zone);
                True(C(backup).IsTapped);
            }
        }
    }
}
