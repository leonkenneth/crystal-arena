namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class GildedDrake
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void ExchangeForForce()
            {
                var drake = C("Gilded Drake");
                var force = C("Verdant Force");

                Hand(P1, drake);
                Battlefield(P1, "Island", "Island");
                Battlefield(P2, force);

                RunGame(1);

                Equal(P1, C(force).Controller);
                Equal(P2, C(drake).Controller);
            }

            [Fact(Skip = "Old card")]
            public void ProtectForce()
            {
                var drake = C("Gilded Drake");
                var force = C("Verdant Force");

                Hand(P1, drake);
                Hand(P2, "Vines of Vastwood");

                Battlefield(P1, "Island", "Island");
                Battlefield(P2, force, "Forest", "Forest");

                RunGame(1);

                Equal(P2, C(force).Controller);
                Equal(Zone.BreakZone, C(drake).Zone);
            }
        }

        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void NoValidTarget()
            {
                var drake = C("Gilded Drake");
                var guma = C("Guma");

                Hand(P1, drake);
                Battlefield(P2, guma);

                Exec(
                    At(Step.FirstMain).Cast(drake).NoValidTarget(),
                    At(Step.SecondMain)
                        .Verify(() =>
                        {
                            Equal(Zone.BreakZone, C(drake).Zone);
                        })
                );
            }
        }
    }
}
