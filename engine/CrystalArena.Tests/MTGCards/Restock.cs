namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Restock
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void ReturnDragons()
            {
                var dragon1 = C("Shivan Dragon");
                var dragon2 = C("Shivan Dragon");
                var restock = C("Restock");

                BreakZone(P1, "Grizzly Bears", dragon1, dragon2);
                Hand(P1, restock);
                Battlefield(P1, "Mountain", "Forest", "Forest", "Forest", "Mountain", "Forest");

                RunGame(1);

                Equal(Zone.RemovedFromPlay, C(restock).Zone);
                Equal(Zone.Hand, C(dragon1).Zone);
                Equal(Zone.Hand, C(dragon2).Zone);
            }
        }
    }
}
