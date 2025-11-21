namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class DayOfJudgment
    {
        public class Predefined : PredefinedAiScenario
        {
            [Fact(Skip = "Old card")]
            public void DestroyAllForwards()
            {
                var day = C("Day of Judgment");

                Battlefield(P1, "Forest", "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");
                Battlefield(P2, "Forest", "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

                Hand(P1, day);

                Exec(
                    At(Step.FirstMain)
                        .Cast(day)
                        .Verify(() =>
                        {
                            Equal(1, P1.Battlefield.Count());
                            Equal(1, P2.Battlefield.Count());
                        })
                );
            }
        }
    }
}
