namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class TimeSpiral
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void CastSpiral()
            {
                var spiral = C("Time Spiral");
                Hand(P1, spiral);
                Hand(P2, "Forest", "Forest", "Forest", "Forest");

                Battlefield(P1, "Island", "Island", "Island", "Island", "Island", "Island");
                BreakZone(P2, "Forest");
                BreakZone(P1, "Forest");

                RunGame(1);

                Equal(Zone.RemovedFromPlay, C(spiral).Zone);
                Equal(0, P1.BreakZone.Count);
                True(P1.Hand.Count >= 6); // Might play and draw backup
                True(P1.Battlefield.Backups.Count(x => !x.IsTapped) >= 6);

                Equal(7, P2.Hand.Count);
                Equal(0, P2.BreakZone.Count);
            }
        }
    }
}
