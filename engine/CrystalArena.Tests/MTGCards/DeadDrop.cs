namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class DeadDrop
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void OpponentSacrificesTwoForwards()
            {
                P1.Life = 4;
                Hand(P1, "Dead Drop");
                Battlefield(
                    P1,
                    "Swamp",
                    "Mountain",
                    "Mountain",
                    "Mountain",
                    "Swamp",
                    "Mountain",
                    "Mountain",
                    "Mountain",
                    "Swamp",
                    "Mountain",
                    "Mountain",
                    "Mountain"
                );

                Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Profane Memento");

                RunGame(1);

                Equal(0, P2.Battlefield.Forwards.Count());
            }
        }
    }
}
