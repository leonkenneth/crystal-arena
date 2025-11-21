namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class MagmaJet
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PutBackupOnTopPutDragonOnBottom()
            {
                var mountain = C("Mountain");
                var dragon = C("Shivan Dragon");
                var elves = C("Llanowar Elves");

                Hand(P1, "Magma Jet");
                MainDeck(P1, dragon, mountain);
                Battlefield(P1, "Mountain", "Forest");
                Battlefield(P2, elves);

                RunGame(1);

                Equal(Zone.BreakZone, C(elves).Zone);
                Equal(C(mountain), P1.MainDeck.Top);
                Equal(C(dragon), P1.MainDeck.Bottom);
            }
        }
    }
}
