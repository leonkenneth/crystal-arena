namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class ChordOfCalling
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void SearchForDragon()
            {
                var dragon = C("Shivan Dragon");

                Hand(P1, "Chord of Calling");
                MainDeck(P1, "Mountain", dragon);

                Battlefield(
                    P1,
                    "Grizzly Bears",
                    "Grizzly Bears",
                    "Grizzly Bears",
                    "Grizzly Bears",
                    "Forest",
                    "Mountain",
                    "Forest",
                    "Forest",
                    "Mountain"
                );

                Battlefield(P2, "Shivan Dragon");

                RunGame(2);

                Equal(Zone.Battlefield, C(dragon).Zone);
            }

            [Fact(Skip = "Old card")]
            public void MysticShouldNotBeCountedForConvokeAndItsManaAbilityBug()
            {
                var dragon = C("Shivan Dragon");

                Hand(P1, "Chord of Calling");
                MainDeck(P1, "Mountain", dragon);

                Battlefield(
                    P1,
                    "Elvish Mystic",
                    "Forest",
                    "Forest",
                    "Forest",
                    "Mountain",
                    "Forest",
                    "Forest",
                    "Mountain"
                );

                Battlefield(P2, "Shivan Dragon");

                RunGame(2);

                Equal(Zone.MainDeck, C(dragon).Zone);
            }
        }
    }
}
