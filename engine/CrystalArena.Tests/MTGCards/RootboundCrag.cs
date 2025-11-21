namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class RootboundCrag
    {
        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void ComesIntoPlayTapped()
            {
                var crag = C("Rootbound Crag");

                Hand(P1, crag);
                Battlefield(P1);

                Exec(At(Step.FirstMain).Cast(crag).Verify(() => True(C(crag).IsTapped)));
            }

            [Fact(Skip = "Old card")]
            public void ComesIntoPlayUntapped()
            {
                var crag = C("Rootbound Crag");

                Hand(P1, crag);
                Battlefield(P1, "Forest");

                Exec(At(Step.FirstMain).Cast(C(crag)).Verify(() => False(C(crag).IsTapped)));
            }
        }
    }
}
