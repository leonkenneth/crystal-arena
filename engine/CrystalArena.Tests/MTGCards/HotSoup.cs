namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class HotSoup
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void DestroyedEquipedForward()
            {
                var bears = C("Grizzly Bears");

                Battlefield(P1, "Llanowar Elves");
                Battlefield(P2, bears.IsEquipedWith("Hot Soup"));

                P1.Life = 2;
                P2.Life = 1;

                RunGame(2);

                Equal(Zone.BreakZone, C(bears).Zone);
            }
        }
    }
}
