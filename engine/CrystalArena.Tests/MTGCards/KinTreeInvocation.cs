namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class KinTreeInvocation
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Put22Counter()
            {
                Hand(P1, "Kin-Tree Invocation");
                Battlefield(
                    P1,
                    "Typhoid Rats",
                    "Grizzly Bears",
                    "Swamp",
                    "Forest",
                    "Forest",
                    "Forest"
                );

                RunGame(1);

                Equal(2, P1.Battlefield.Forwards.Count(x => x.Power == 2));
            }
        }
    }
}
