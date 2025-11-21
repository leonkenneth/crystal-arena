namespace CrystalArena.Tests.FFTCGCards.Opus23
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Opus23_010C_Soldier : PredefinedScenario
    {
        [Fact]
        public void GainPower()
        {
            var soldier = C("23-010C");
            BreakZone(P1, "23-010C", "23-010C");
            Battlefield(P1, soldier);

            Exec(
                At(Step.FirstMain, turn: 1)
                    .Verify(() =>
                    {
                        Equal(8000, soldier.Card.Power);
                    })
            );
        }
    }
}
