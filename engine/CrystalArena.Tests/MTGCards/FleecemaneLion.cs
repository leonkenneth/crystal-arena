namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class FleecemaneLion
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void BecomeMonstrous()
            {
                var lion = C("Fleecemane Lion");
                Battlefield(P1, lion, "Forest", "Forest", "Forest", "Plains", "Plains");

                False(C(lion).Has().Hexproof);

                RunGame(3);

                True(C(lion).Has().Hexproof);
                True(C(lion).Has().Monstrosity);
                Equal(4, C(lion).Power);
            }
        }
    }
}
