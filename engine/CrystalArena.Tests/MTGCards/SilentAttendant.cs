namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class SilentAttendant
    {
        public class PredefinedAi : PredefinedAiScenario
        {
            [Fact(Skip = "Old card")]
            public void ActivateEot()
            {
                var attendant = C("Silent Attendant");

                Battlefield(P2, attendant);

                Exec(At(Step.FirstMain, 2).Verify(() => Equal(21, P2.Life)));
            }
        }
    }
}
