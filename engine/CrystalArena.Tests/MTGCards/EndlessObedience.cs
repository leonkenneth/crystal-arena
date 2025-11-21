namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class EndlessObedience
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void ReturnForceFromOpponentsBreakZone()
            {
                var force = C("Verdant Force");

                Battlefield(P1, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
                Hand(P1, "Endless Obedience");

                BreakZone(P2, force);

                RunGame(1);

                Equal(Zone.Battlefield, C(force).Zone);
                Equal(P1, C(force).Controller);
            }

            [Fact(Skip = "Old card")]
            public void EquipementShouldNotChangeBattlefieldsBug()
            {
                Hand(P1, "Endless Obedience");
                Battlefield(
                    P1,
                    "Brawler's Plate",
                    "Swamp",
                    "Swamp",
                    "Swamp",
                    "Swamp",
                    "Swamp",
                    "Swamp"
                );

                P2.Life = 7;
                BreakZone(P2, "Shivan Dragon");
                Battlefield(P2, "Plains", "Plains", "Plains", "Plains");
                Hand(P2, "Divine Verdict");

                RunGame(3);
            }
        }
    }
}
