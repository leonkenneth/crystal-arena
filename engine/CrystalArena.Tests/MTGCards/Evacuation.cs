namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Evacuation
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void ReturnAllForwardsToHands()
            {
                Battlefield(P1, "Rumbling Slum", "Verdant Force", "Verdant Force");
                Battlefield(P2, "Wall of Denial", "Island", "Island", "Island", "Island", "Island");
                Hand(P2, "Evacuation");

                RunGame(1);

                Equal(0, P2.Battlefield.Forwards.Count());
                Equal(0, P1.Battlefield.Forwards.Count());
            }
        }
    }
}
