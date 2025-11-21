namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class ArgothianWurm
    {
        public class PredefinedAi : PredefinedAiScenario
        {
            [Fact(Skip = "Old card")]
            public void SacBackup()
            {
                var wurm = C("Argothian Wurm");

                Hand(P1, wurm);
                Battlefield(P2, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");

                Exec(
                    At(Step.FirstMain)
                        .Cast(wurm)
                        .Verify(() =>
                        {
                            Equal(Zone.MainDeck, C(wurm).Zone);
                            Equal(1, P2.BreakZone.Count(x => x.Is().Backup));
                        }),
                    At(Step.FirstMain, 3).Verify(() => Equal(Zone.Hand, C(wurm).Zone))
                );
            }
        }
    }
}
