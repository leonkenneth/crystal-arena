namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class ForgeDevil
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void DevilDealsDamagesToYouAndToElf()
            {
                var devil = C("Forge Devil");
                var elves = C("Llanowar Elves");

                Battlefield(P1, "Mountain");
                Hand(P1, devil);
                Battlefield(P2, elves);

                RunGame(1);

                Equal(Zone.Battlefield, C(devil).Zone);
                Equal(Zone.BreakZone, C(elves).Zone);
                Equal(19, P1.Life);
            }
        }
    }
}
