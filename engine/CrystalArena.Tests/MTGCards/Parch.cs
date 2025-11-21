namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Parch
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Deal4DamageToBlueForward()
            {
                var serpent = C("Sandbar Serpent");

                Hand(P1, "Parch");
                Battlefield(P1, "Mountain", "Mountain");

                Battlefield(P2, "Grizzly Bears", serpent);

                RunGame(1);

                Equal(Zone.BreakZone, C(serpent).Zone);
            }
        }
    }
}
