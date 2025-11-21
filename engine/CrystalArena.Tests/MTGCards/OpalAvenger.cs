namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class OpalAvenger
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void TurnIntoForwardImmediately()
            {
                var avenger = C("Opal Avenger");

                Hand(P1, avenger);
                Battlefield(P1, "Plains", "Swamp", "Swamp");
                P1.Life = 10;

                RunGame(1);

                True(C(avenger).Is().Forward);
            }
        }
    }
}
