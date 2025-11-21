namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class GoblinMedics
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void KillPegasusDeal1Damage()
            {
                var pegasus = C("Pegasus Charger");
                Battlefield(P1, "Goblin Medics");
                Battlefield(P2, pegasus);

                RunGame(1);

                Equal(Zone.BreakZone, C(pegasus).Zone);
                Equal(19, P2.Life);
            }
        }
    }
}
