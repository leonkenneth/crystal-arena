namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class KalonianTwinCrystalArena
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void CrystalArenaAndTwinAre66()
            {
                Hand(P1, "Kalonian TwinCrystalArena");
                Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Forest", "Forest");
                RunGame(1);

                Equal(2, P1.Battlefield.Forwards.Count(x => x.Power == 6 && x.Toughness == 6));
            }
        }
    }
}
