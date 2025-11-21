namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class TormodsCrypt
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PreventGravediggerFromBringingForwardsBack()
            {
                Battlefield(P1, "Tormod's Crypt");

                Hand(P2, "Gravedigger");
                Battlefield(P2, "Swamp", "Plains", "Plains", "Plains", "Plains");
                BreakZone(P2, "Serra Angel", "Shivan Dragon", "Plains", "Plains");

                RunGame(2);

                Equal(0, P2.BreakZone.Count);
            }
        }
    }
}
