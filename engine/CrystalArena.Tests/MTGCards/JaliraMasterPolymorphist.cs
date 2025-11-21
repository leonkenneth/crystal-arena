namespace CrystalArena.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class JaliraMasterPolymorphist
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void PutForceIntoPlay()
            {
                var force = C("Verdant Force");
                var forest = C("Forest");

                MainDeck(P1, forest, force, "Swamp");
                Battlefield(
                    P1,
                    "Jalira, Master Polymorphist",
                    C("Grizzly Bears").IsEnchantedWith("Pacifism"),
                    "Island",
                    "Island",
                    "Island"
                );

                Battlefield(P2, "Wall of Frost");

                P2.Life = 2;

                RunGame(2);

                Equal(Zone.Battlefield, C(force).Zone);
                Equal(Zone.MainDeck, C(forest).Zone);
            }

            [Fact(Skip = "Old card")]
            public void PutAllOnBottomOfMainDeck()
            {
                var legenadry = C("Jalira, Master Polymorphist");
                var forest = C("Forest");

                MainDeck(P1, forest, legenadry, "Swamp");
                Battlefield(
                    P1,
                    "Jalira, Master Polymorphist",
                    C("Grizzly Bears").IsEnchantedWith("Pacifism"),
                    "Island",
                    "Island",
                    "Island"
                );

                Battlefield(P2, "Wall of Frost");

                RunGame(1);

                Equal(Zone.MainDeck, C(legenadry).Zone);
            }
        }
    }
}
