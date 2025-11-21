namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Juggernaut
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void AttackWallWithJuggernaut()
            {
                Battlefield(P1, "Juggernaut");

                P2.Life = 5;
                Battlefield(P2, "Wall of Fire");

                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
