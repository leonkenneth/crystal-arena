namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class TauntingElf
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void AttackWithAll()
            {
                Battlefield(P1, "Verdant Force", "Taunting Elf", "Verdant Force");
                Battlefield(P2, "Wall of Blossoms", "Wall of Blossoms", "Grizzly Bears");
                P2.Life = 14;

                RunGame(1);

                Equal(0, P2.Life);
            }

            [Fact(Skip = "Old card")]
            public void DoesNotWorkSoWellIfItHasFlying()
            {
                Battlefield(
                    P1,
                    "Verdant Force",
                    C("Taunting Elf").IsEnchantedWith("Launch"),
                    "Verdant Force"
                );
                Battlefield(P2, "Wall of Blossoms", "Wall of Blossoms");
                P2.Life = 14;

                RunGame(1);

                Equal(14, P2.Life);
            }
        }
    }
}
