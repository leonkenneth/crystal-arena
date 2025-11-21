namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class AuraFlux
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void EchantmentsGainUpkeep()
            {
                var worship = C("Worship");
                Battlefield(P1, "Aura Flux");
                Battlefield(P2, worship);

                RunGame(2);

                Equal(Zone.BreakZone, C(worship).Zone);
            }
        }
    }
}
