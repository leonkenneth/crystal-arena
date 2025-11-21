namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Metalworker
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void CastEngine()
            {
                Hand(P1, "Wurmcoil Engine", "Wurmcoil Engine", "Wurmcoil Engine");
                Battlefield(P1, "Metalworker");

                RunGame(1);

                Equal(1, P1.Battlefield.Count(c => c.Name == "Wurmcoil Engine"));
            }
        }
    }
}
