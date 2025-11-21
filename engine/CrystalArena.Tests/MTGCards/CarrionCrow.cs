namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class CarrionCrow
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PutIntoPlay()
            {
                Hand(P1, "Carrion Crow");
                Battlefield(P1, "Swamp", "Swamp", "Swamp");

                RunGame(1);

                Equal(1, P1.Battlefield.Forwards.Count(c => c.IsTapped));
            }
        }
    }
}
