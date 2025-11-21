namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Gamekeeper
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PutForceIntoPlay()
            {
                var force = C("Verdant Force");
                var forest = C("Forest");
                var gamekeeper = C("Gamekeeper");

                MainDeck(P1, forest, force, "Swamp");
                Battlefield(P1, gamekeeper);
                Battlefield(P2, "Grizzly Bears");

                P2.Life = 2;

                RunGame(1);

                Equal(Zone.Battlefield, C(force).Zone);
                Equal(Zone.BreakZone, C(forest).Zone);
                Equal(Zone.RemovedFromPlay, C(gamekeeper).Zone);
            }
        }
    }
}
