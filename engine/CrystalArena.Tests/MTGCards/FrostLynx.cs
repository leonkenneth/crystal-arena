namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class FrostLynx
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void TapForward()
            {
                var host = C("Blood Host");

                Hand(P1, "Frost Lynx");
                Battlefield(P1, "Island", "Island", "Island");

                Battlefield(P2, host);
                RunGame(2);

                True(C(host).IsTapped);
            }
        }
    }
}
