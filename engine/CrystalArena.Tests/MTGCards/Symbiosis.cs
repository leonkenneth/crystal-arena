namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Symbiosis
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void AttackFor8()
            {
                Hand(P1, "Symbiosis");
                Battlefield(P1, "Grizzly Bears", "Grizzly Bears", "Forest", "Forest");

                RunGame(1);

                Equal(12, P2.Life);
            }

            [Fact(Skip = "Old card")]
            public void AttackFor2Only()
            {
                Hand(P1, "Symbiosis");
                Battlefield(P1, "Grizzly Bears", "Forest", "Forest");

                RunGame(1);

                Equal(18, P2.Life);
            }
        }
    }
}
