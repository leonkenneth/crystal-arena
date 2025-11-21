namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class CausticTar
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void EnchantABackupAndTapAtEot()
            {
                Hand(P1, "Caustic Tar");
                Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");

                RunGame(4);

                Equal(17, P2.Life);
            }
        }
    }
}
