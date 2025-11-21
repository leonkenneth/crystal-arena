namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class FodderCannon
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void KillWallandWin()
            {
                Battlefield(
                    P1,
                    "Trained Armodon",
                    "Llanowar Elves",
                    "Fodder Cannon",
                    "Island",
                    "Forest",
                    "Island",
                    "Forest"
                );
                Battlefield(P2, "Wall of Blossoms");
                P2.Life = 3;

                RunGame(1);

                Equal(0, P2.Life);
                Equal(1, P1.BreakZone.Count);
                Equal(1, P2.BreakZone.Count);
            }
        }
    }
}
