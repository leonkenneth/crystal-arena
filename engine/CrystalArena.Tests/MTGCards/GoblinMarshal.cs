namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class GoblinMarshal
    {
        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void Get4Tokens()
            {
                var marshal = C("Goblin Marshal");

                Hand(P1, marshal);

                Exec(
                    At(Step.FirstMain).Cast(marshal),
                    At(Step.SecondMain)
                        .Verify(() => Equal(2, P1.Battlefield.Count(x => x.Is().Token))),
                    At(Step.FirstMain, turn: 3)
                        .Verify(() => Equal(4, P1.Battlefield.Count(x => x.Is().Token)))
                );
            }
        }
    }
}
