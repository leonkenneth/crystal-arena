namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Masticore
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void DiscardCardKillBearAttack()
            {
                Hand(P1, "Island");
                Battlefield(P1, "Masticore", "Island", "Island", "Island", "Island");
                Battlefield(P2, "Grizzly Bears");
                P2.Life = 4;

                RunGame(1);

                Equal(1, P1.BreakZone.Count(c => c.Is().Backup));
                Equal(0, P2.Life);
            }
        }
    }
}
