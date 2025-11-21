namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class ChasmSkulker
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void WhenP2DrawsSkulkerGets11Counter()
            {
                var skulker = C("Chasm Skulker");

                MainDeck(P1, "Swamp");
                MainDeck(P2, "Island");
                Battlefield(P2, skulker);

                RunGame(3);

                Equal(2, C(skulker).Power);
            }

            [Fact(Skip = "Old card")]
            public void DestroySkulkerCreateTokens()
            {
                var skulker = C("Chasm Skulker");

                P1.Life = 2;
                Battlefield(P1, "Grizzly Bears");

                MainDeck(P2, "Island");
                Battlefield(P2, skulker);

                RunGame(3);

                Equal(0, P1.Battlefield.Count);
                True(C(skulker).Zone == Zone.BreakZone);
                Equal(1, P2.Battlefield.Forwards.Count(c => c.Is().Token));
            }
        }
    }
}
