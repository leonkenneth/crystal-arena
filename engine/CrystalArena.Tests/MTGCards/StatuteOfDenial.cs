namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class StatuteOfDenial
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void CounterBoltAndDrawDragon()
            {
                var dragon = C("Shivan Dragon");
                var bolt = C("Lightning Bolt");

                Hand(P1, "Statute of Denial", "Grizzly Bears", "Grizzly Bears");

                MainDeck(P1, dragon);
                Battlefield(P1, "Island", "Island", "Island", "Island", "Fugitive Wizard");

                Hand(P2, bolt);
                Battlefield(P2, "Mountain");

                P1.Life = 3;

                RunGame(1);

                Equal(3, P1.Life);
                Equal(Zone.BreakZone, C(bolt).Zone);
                Equal(Zone.Hand, C(dragon).Zone);
            }
        }
    }
}
