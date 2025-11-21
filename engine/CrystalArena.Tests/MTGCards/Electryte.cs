namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class Electryte
    {
        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void DealDamageToBlockers()
            {
                var armodon1 = C("Trained Armodon");
                var armodon2 = C("Trained Armodon");
                var bear = C("Grizzly Bears");
                var electryte = C("Electryte");

                Battlefield(P1, electryte, bear);
                Battlefield(P2, armodon1, armodon2);

                Exec(
                    At(Step.DeclareAttackers).DeclareAttackers(electryte, bear),
                    At(Step.DeclareBlocker).DeclareBlockers(bear, armodon1, bear, armodon2),
                    At(Step.SecondMain)
                        .Verify(() =>
                        {
                            Equal(Zone.BreakZone, C(armodon1).Zone);
                            Equal(Zone.BreakZone, C(armodon2).Zone);
                        })
                );
            }
        }
    }
}
