namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class IvorytuskFortress
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void UntapForwardsWithCounters()
            {
                var vebulid = C("Vebulid");
                var bear = C("Grizzly Bears");
                Hand(P1, vebulid);
                Battlefield(P1, "Swamp", "Ivorytusk Fortress", bear);

                RunGame(4);

                False(C(vebulid).IsTapped);
                True(C(bear).IsTapped);
            }
        }
    }
}
