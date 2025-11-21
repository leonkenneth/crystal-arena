namespace CrystalArena.Tests.Cards
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class SoulOfInnistrad
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void RemoveFromPlayFromBreakZoneToReturn3ForwardsToHand()
            {
                var soul = C("Soul of Innistrad");
                BreakZone(P1, soul, "Shivan Dragon", "Shivan Dragon", "Shivan Dragon");
                Battlefield(P1, "Swamp", "Swamp", "Mountain", "Mountain", "Mountain");

                RunGame(2);

                Equal(3, P1.Hand.Count(x => x.Name == "Shivan Dragon"));
                Equal(Zone.RemovedFromPlay, C(soul).Zone);
            }
        }
    }
}
