namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class JorubaiMurkLurker
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void DragonGetsLifelink()
            {
                Battlefield(P1, "Jorubai Murk Lurker", "Shivan Dragon", "Swamp", "Swamp");

                RunGame(1);

                Equal(25, P1.Life);
            }
        }
    }
}
