namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class ChargingRhino
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void CannotBeBlockedBy2()
            {
                Battlefield(P1, "Charging Rhino");
                Battlefield(P2, "Centaur Courser", "Centaur Courser");

                P2.Life = 4;

                RunGame(1);

                Equal(0, P1.BreakZone.Count());
                Equal(1, P2.BreakZone.Count());
            }
        }
    }
}
