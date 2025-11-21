namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class BattleBrawler
    {
        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void Gets10AndFirstStrike()
            {
                var brawler = C("Battle Brawler");
                var alarm = C("Raise The Alarm");

                Hand(P1, alarm);
                Battlefield(P1, brawler);

                Exec(
                    At(Step.Upkeep)
                        .Verify(() =>
                        {
                            Equal(2, C(brawler).Power);
                            False(C(brawler).Has().FirstStrike);
                        }),
                    At(Step.FirstMain)
                        .Cast(alarm)
                        .Verify(() =>
                        {
                            Equal(3, C(brawler).Power);
                            True(C(brawler).Has().FirstStrike);
                        })
                );
            }
        }
    }
}
