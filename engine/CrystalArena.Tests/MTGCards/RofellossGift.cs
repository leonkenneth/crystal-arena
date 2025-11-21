namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class RofellossGift
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Return2Rancors()
            {
                Battlefield(P1, "Grizzly Bears", "Forest", "Forest", "Forest");
                Hand(P1, "Grizzly Bears", "Grizzly Bears", "Rofellos's Gift");
                BreakZone(P1, "Rancor", "Rancor");
                P2.Life = 6;

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
