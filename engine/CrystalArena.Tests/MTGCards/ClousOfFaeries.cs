namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class ClousOfFaeries
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Untap2Backups()
            {
                Hand(P1, "Cloud of Faeries");
                Battlefield(P1, "Remote Isle", "Forest", "Forest");

                RunGame(1);

                Equal(1, P1.Battlefield.Forwards.Count());
                Equal(0, P1.Battlefield.Backups.Count(x => x.IsTapped));
            }
        }
    }
}
