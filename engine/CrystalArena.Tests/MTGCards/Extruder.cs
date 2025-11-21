namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Extruder
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void SacrificeBloodToWin()
            {
                var dragonBlood = C("Dragon Blood");
                Battlefield(P1, "Extruder", dragonBlood, "Island", "Island", "Island", "Island");
                P2.Life = 5;

                RunGame(1);
                Equal(0, P2.Life);
                Equal(Zone.BreakZone, C(dragonBlood).Zone);
            }
        }
    }
}
