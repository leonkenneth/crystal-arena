namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class AeronautTinkerer
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void GetsFlying()
            {
                var tinker = C("Aeronaut Tinkerer");
                Hand(P1, "Profane Memento");
                Battlefield(P1, tinker, "Forest");

                RunGame(1);

                True(C(tinker).Has().Flying);
            }

            [Fact(Skip = "Old card")]
            public void HasNotFlying()
            {
                var tinker = C("Aeronaut Tinkerer");
                Battlefield(P1, tinker);

                RunGame(1);

                False(C(tinker).Has().Flying);
            }
        }
    }
}
