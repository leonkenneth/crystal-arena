namespace CrystalArena.Tests.FFTCGCards.Opus1
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Opus1_107L_Shantotto : PredefinedScenario
    {
        [Fact]
        public void RemovesAllForwardsFromGameOnEnter()
        {
            var shantotto = C("1-107L");
            var myForward = C("0-002X");
            var opponentForward = C("0-002X");
            Battlefield(P1, myForward);
            Battlefield(P2, opponentForward);
            Hand(P1, shantotto);

            Exec(
                At(Step.FirstMain, turn: 1).Cast(shantotto),
                At(Step.SecondMain, turn: 1)
                    .Verify(() =>
                    {
                        True(myForward.Card.Zone() == Zone.RemovedFromPlay);
                        True(opponentForward.Card.Zone() == Zone.RemovedFromPlay);
                    })
            );
        }

        [Fact]
        public void GainsAllSixElementsWhileOnField()
        {
            var shantotto = C("1-107L");
            Battlefield(P1, shantotto);

            Exec(
                At(Step.FirstMain, turn: 1)
                    .Verify(() =>
                    {
                        True(shantotto.Card.HasColor(CardColor.Fire));
                        True(shantotto.Card.HasColor(CardColor.Ice));
                        True(shantotto.Card.HasColor(CardColor.Wind));
                        True(shantotto.Card.HasColor(CardColor.Earth));
                        True(shantotto.Card.HasColor(CardColor.Lightning));
                        True(shantotto.Card.HasColor(CardColor.Water));
                        False(shantotto.Card.HasColor(CardColor.Light));
                        False(shantotto.Card.HasColor(CardColor.Dark));
                    })
            );
        }

        [Fact]
        public void OnlyHasItsPrintedElementWhileOffTheField()
        {
            var shantotto = C("1-107L");
            Hand(P1, shantotto);

            // The six extra Elements come from an on-field static ability, so off the
            // field Shantotto keeps only its printed Earth Element.
            Exec(
                At(Step.FirstMain, turn: 1)
                    .Verify(() =>
                    {
                        True(shantotto.Card.HasColor(CardColor.Earth));
                        False(shantotto.Card.HasColor(CardColor.Fire));
                        False(shantotto.Card.HasColor(CardColor.Water));
                    })
            );
        }
    }
}
