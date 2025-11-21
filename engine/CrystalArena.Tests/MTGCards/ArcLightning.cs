namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class ArcLightning
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void Kill2ForwardsDamagePlayer()
            {
                var elf = C("Llanowar Elves");
                var bird = C("Birds of Paradise");

                var arc = C("Arc Lightning");

                Hand(P1, arc);
                Battlefield(P1, "Mountain", "Mountain", "Mountain");
                Battlefield(P2, elf, bird);

                RunGame(1);

                Equal(Zone.BreakZone, C(elf).Zone);
                Equal(Zone.BreakZone, C(bird).Zone);
                Equal(19, P2.Life);
            }

            [Fact(Skip = "Old card")]
            public void DoNotDmgOwnForwards()
            {
                var elf = C("Llanowar Elves");
                var bird = C("Birds of Paradise");

                var arc = C("Arc Lightning");

                Hand(P1, arc);
                Battlefield(P1, bird, "Mountain", "Mountain", "Mountain");
                Battlefield(P2, elf);

                RunGame(1);

                Equal(Zone.BreakZone, C(elf).Zone);
                Equal(18, P2.Life);
            }

            [Fact(Skip = "Old card")]
            public void KillPlayer()
            {
                var elf = C("Llanowar Elves");
                var bird = C("Birds of Paradise");

                var arc = C("Arc Lightning");

                Hand(P1, arc);
                Battlefield(P1, "Mountain", "Mountain", "Mountain");
                Battlefield(P2, elf, bird);

                P2.Life = 3;
                RunGame(1);

                Equal(0, P2.Life);
            }
        }
    }
}
