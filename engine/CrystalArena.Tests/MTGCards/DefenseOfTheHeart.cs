namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class DefenseOfTheHeart
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Search2Forwards()
            {
                var force1 = C("Verdant Force");
                var force2 = C("Verdant Force");
                var heart = C("Defense of the Heart");

                MainDeck(P1, "Forest", force1, force2);
                Battlefield(P1, heart);
                Battlefield(P2, "Grizzly Bears", "Llanowar Elves", "Birds of Paradise");

                RunGame(1);

                Equal(Zone.Battlefield, C(force1).Zone);
                Equal(Zone.Battlefield, C(force2).Zone);
                Equal(Zone.BreakZone, C(heart).Zone);
            }
        }
    }
}
