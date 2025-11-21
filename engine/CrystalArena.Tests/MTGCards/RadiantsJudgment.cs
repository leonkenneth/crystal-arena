namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class RadiantsJudgment
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void KillForce()
            {
                var force = C("Verdant Force");

                Hand(P1, "Radiant's Judgment");
                Battlefield(P1, "Plains", "Plains", "Plains");
                Battlefield(P2, force);

                RunGame(1);

                Equal(Zone.BreakZone, C(force).Zone);
            }
        }
    }
}
