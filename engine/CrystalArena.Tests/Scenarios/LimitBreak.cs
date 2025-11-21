namespace CrystalArena.Tests.Scenarios
{
    using System.Linq;
    using CrystalArena.Infrastructure;
    using Infrastructure;
    using Xunit;

    public class LimitBreak
    {
        public class Predefined : PredefinedScenario
        {
            [Fact]
            public void LimitBreakZoneExists()
            {
                var limitBreakForward = C("0-006X");
                LimitBreak(P1, limitBreakForward);

                Exec(At(Step.FirstMain).Verify(() => Equal(1, P1.LimitBreak.Count)));
            }

            [Fact]
            public void CanCastFromLimitBreakInMainPhase()
            {
                var limitBreakForward1 = C("0-006X");
                var limitBreakForward2 = C("0-006X");
                var limitBreakForward3 = C("0-006X");
                var forward = C("0-002X");
                Hand(P1, forward);
                LimitBreak(P1, limitBreakForward1, limitBreakForward2, limitBreakForward3);

                Exec(
                    At(Step.FirstMain)
                        .Verify(() =>
                        {
                            Equal(1, limitBreakForward1.Card.CanCast().Count);
                        }),
                    At(Step.FirstMain, turn: 2)
                        .Verify(() =>
                        {
                            Equal(0, limitBreakForward1.Card.CanCast().Count);
                        })
                );
            }

            [Fact]
            public void CannotCastIfNoLBLeft()
            {
                var limitBreakForward = C("0-006X");
                var forward = C("0-002X");
                Hand(P1, forward);
                LimitBreak(P1, limitBreakForward);

                Exec(
                    At(Step.FirstMain)
                        .Verify(() =>
                        {
                            Equal(0, limitBreakForward.Card.CanCast().Count);
                        })
                );
            }

            [Fact]
            public void RevealsLBCardsToCast()
            {
                var limitBreakForward1 = C("0-006X");
                var limitBreakForward2 = C("0-006X");
                var limitBreakForward3 = C("0-006X");
                var forward = C("0-002X");
                Hand(P1, forward);
                LimitBreak(P1, limitBreakForward1, limitBreakForward2, limitBreakForward3);

                Exec(
                    At(Step.FirstMain)
                        .Cast(
                            limitBreakForward1,
                            targets: Ts(),
                            costTargets: Ts(limitBreakForward2, limitBreakForward3)
                        )
                        .Verify(() =>
                        {
                            Equal(1, P1.Battlefield.Count); // LB Forward was successfully cast
                            Equal(2, P1.Hand.Count); // Card was discarded for CP, but we draw 2 cards at the beginning of turn
                            True(limitBreakForward2.Card.IsRevealed); // LB 2 means we revealed 2 cards
                            True(limitBreakForward3.Card.IsRevealed);
                        })
                );
            }
        }
    }
}
