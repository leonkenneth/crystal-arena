namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class GoblinRabblemaster
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void GetTokenWithHasteAndAttackFor4()
            {
                Battlefield(P1, "Goblin Rabblemaster");

                RunGame(1);

                Equal(16, P2.Life);
            }
        }
    }
}
