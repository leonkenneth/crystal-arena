namespace CrystalArena.Tests.FFTCGCards.Opus23
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Opus23_003C_Kain
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void SpecialAbilityCannotBePaidWithoutOtherCardWithSameNameInHand()
            {
                var kain = C("23-003C");
                var target = C("23-001C");

                Battlefield(P1, kain);
                Battlefield(P2, target);

                // Without another Kain in hand, the ability cannot be activated
                Exec(
                    At(Step.FirstMain, turn: 1)
                        .Verify(() =>
                        {
                            Equal(0, kain.Card.CanActivateAbilities().Count);
                        })
                );
            }

            [Fact]
            public void SpecialAbilityCanBePaidWithOtherCardWithSameNameInHandAndMana()
            {
                var kain = C("23-003C");
                var kainInHand = C("23-003C");
                var otherCardForMana = C("0-001X");

                Battlefield(P1, kain);
                Hand(P1, kainInHand, otherCardForMana);

                Exec(
                    At(Step.FirstMain, turn: 1)
                        .Verify(() =>
                        {
                            Equal(1, kain.Card.CanActivateAbilities().Count);
                        })
                );
            }
        }
    }
}
