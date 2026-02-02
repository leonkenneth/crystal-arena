namespace CrystalArena.Tests.FFTCGCards.Opus23
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Opus23_025C_Shelke : PredefinedScenario
    {
        [Fact]
        public void GainCrystalTriggersOnlyOncePerTurn()
        {
            var weiss = C("23-021C");
            var shelke = C("23-025C");
            Hand(P1, weiss, shelke);

            Exec(
                At(Step.FirstMain, turn: 1)
                    .Verify(() =>
                    {
                        False(P1.HasMana("{Z}".Parse()));
                    })
                    .Cast(shelke)
                    .Verify(() =>
                    {
                        True(P1.HasMana("{Z}".Parse()));
                        False(P1.HasMana("{Z}{Z}".Parse()));
                    })
                    .Cast(weiss)
                    .Verify(() =>
                    {
                        // Weiss AI enters the field consumes the crystal
                        False(P1.HasMana("{Z}".Parse()));
                        False(P1.HasMana("{Z}{Z}".Parse()));
                    })
            );
        }

        [Fact]
        public void OpponentShelkeDoesNotTriggerMyShelke()
        {
            var myShelke = C("23-025C");
            var opponentShelke = C("23-025C");
            Hand(P1, myShelke);
            Hand(P2, opponentShelke);

            Exec(
                At(Step.FirstMain, turn: 1)
                    .Cast(myShelke)
                    .Verify(() =>
                    {
                        True(P1.HasMana("{Z}".Parse()));
                    }),
                At(Step.FirstMain, turn: 2)
                    .Cast(opponentShelke)
                    .Verify(() =>
                    {
                        // P1's Shelke should not trigger from P2's Shelke entering
                        False(P1.HasMana("{Z}{Z}".Parse()));
                        True(P1.HasMana("{Z}".Parse()));
                        // P2 should gain crystal from their own Shelke
                        True(P2.HasMana("{Z}".Parse()));
                    })
            );
        }
    }
}
