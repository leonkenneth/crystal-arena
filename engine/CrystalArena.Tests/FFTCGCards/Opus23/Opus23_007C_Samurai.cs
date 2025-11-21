namespace CrystalArena.Tests.FFTCGCards.Opus23
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Opus23_007C_Samurai : PredefinedScenario
    {
        [Fact]
        public void GainCrystal()
        {
            var samurai = C("23-007C");
            Hand(P1, samurai, "23-007C");

            Exec(
                At(Step.FirstMain, turn: 1)
                    .Cast(samurai)
                    .Verify(() =>
                    {
                        Equal(
                            true,
                            P1.ManaCache.Has(
                                "{Z}".Parse(),
                                ManaUsage.Any,
                                new ConvokeAndDelveOptions()
                            )
                        );
                    })
            );
        }

        [Fact]
        public void MultiplayableRule()
        {
            var samurai = C("23-007C");

            Hand(P1, samurai);
            Battlefield(P1, "23-007C");

            Exec(
                At(Step.FirstMain).Cast(samurai),
                At(Step.SecondMain).Verify(() => Equal(2, P1.Battlefield.Count()))
            );
        }
    }
}
