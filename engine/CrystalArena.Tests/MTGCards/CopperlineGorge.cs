namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class CopperlineGorge
    {
        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void ComesIntoPlayTapped()
            {
                var gorge = C("Copperline Gorge");

                Hand(P1, gorge);
                Battlefield(P1, "Forest", "Forest", "Forest");

                Exec(At(Step.FirstMain).Cast(gorge).Verify(() => True(C(gorge).IsTapped)));
            }

            [Fact(Skip = "Old card")]
            public void ComesIntoPlayUntapped()
            {
                var gorge = C("Copperline Gorge");

                Hand(P1, gorge);
                Battlefield(P1, "Forest", "Forest");

                Exec(At(Step.FirstMain).Cast(gorge).Verify(() => False(C(gorge).IsTapped)));
            }
        }
    }
}
