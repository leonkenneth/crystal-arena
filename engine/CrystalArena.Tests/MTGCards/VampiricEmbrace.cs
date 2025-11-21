namespace CrystalArena.Tests.Cards
{
    using Infrastructure;
    using Xunit;

    public class VampiricEmbrace
    {
        public class Ai : AiScenario
        {
            [Fact(Skip = "Old card")]
            public void KillOwl()
            {
                var bear = C("Grizzly Bears");
                Battlefield(P1, bear.IsEnchantedWith("Vampiric Embrace"));
                Battlefield(P2, "Spire Owl");

                P2.Life = 4;

                RunGame(1);

                Equal(5, C(bear).Toughness);
            }
        }

        public class Predefined : PredefinedScenario
        {
            [Fact(Skip = "Old card")]
            public void AddCounter()
            {
                var bear = C("Grizzly Bears");
                var owl = C("Spire Owl");

                Battlefield(P1, bear.IsEnchantedWith("Vampiric Embrace"));
                Battlefield(P2, owl);

                Exec(
                    At(Step.DeclareAttackers).DeclareAttackers(bear),
                    At(Step.DeclareBlocker).DeclareBlockers(bear, owl),
                    At(Step.SecondMain).Verify(() => Equal(5, C(bear).Power))
                );
            }

            [Fact(Skip = "Old card")]
            public void DoNotAddCounter()
            {
                var bear = C("Grizzly Bears");
                var wall = C("Wall of Denial");

                Battlefield(P1, bear.IsEnchantedWith("Vampiric Embrace"));
                Battlefield(P2, wall);

                Exec(
                    At(Step.DeclareAttackers).DeclareAttackers(bear),
                    At(Step.DeclareBlocker).DeclareBlockers(bear, wall),
                    At(Step.SecondMain).Verify(() => Equal(4, C(bear).Power))
                );
            }

            [Fact(Skip = "Old card")]
            public void AddCounterSecondSpell()
            {
                var bear = C("Grizzly Bears");
                var wall = C("Wall of Denial");
                var edict = C("Cruel Edict");

                Hand(P1, edict);
                Battlefield(P1, bear.IsEnchantedWith("Vampiric Embrace"));
                Battlefield(P2, wall);

                Exec(
                    At(Step.DeclareAttackers).DeclareAttackers(bear),
                    At(Step.DeclareBlocker).DeclareBlockers(bear, wall),
                    At(Step.SecondMain).Cast(edict).Verify(() => Equal(5, C(bear).Power))
                );
            }

            [Fact(Skip = "Old card")]
            public void DoNotAddCounterNextTurn()
            {
                var bear = C("Grizzly Bears");
                var wall = C("Wall of Denial");
                var edict = C("Cruel Edict");

                Hand(P1, edict);
                Battlefield(P1, bear.IsEnchantedWith("Vampiric Embrace"));
                Battlefield(P2, wall);

                Exec(
                    At(Step.DeclareAttackers).DeclareAttackers(bear),
                    At(Step.DeclareBlocker).DeclareBlockers(bear, wall),
                    At(Step.FirstMain, turn: 3).Cast(edict).Verify(() => Equal(4, C(bear).Power))
                );
            }
        }
    }
}
