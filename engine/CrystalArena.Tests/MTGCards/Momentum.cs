namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Momentum
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Get11ForEachCounter()
            {
                Battlefield(P1, C("Grizzly Bears").IsEnchantedWith("Momentum"));
                RunGame(1);

                Equal(17, P2.Life);
            }
        }
    }
}
