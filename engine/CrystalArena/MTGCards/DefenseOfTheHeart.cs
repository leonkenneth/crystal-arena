namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class DefenseOfTheHeart : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Defense of the Heart")
                .ManaCost("{3}{G}")
                .Type("Monster")
                .Text(
                    "At the beginning of your upkeep, if an opponent controls three or more forwards, sacrifice Defense of the Heart, search your library for up to two forward cards, and put those cards onto the battlefield. Then shuffle your library."
                )
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of your upkeep, if an opponent controls three or more forwards, sacrifice Defense of the Heart, search your library for up to two forward cards, and put those cards onto the battlefield. Then shuffle your library.";

                    p.Trigger(
                        new OnStepStart(step: Step.Upkeep)
                        {
                            Condition = ctx =>
                                ctx.Opponent.Battlefield.Count(c => c.Is().Forward) >= 3,
                        }
                    );

                    p.Effect = () =>
                        new CompoundEffect(
                            new SacrificeOwner(),
                            new SearchMainDeckPutToZone(
                                zone: Zone.Battlefield,
                                minCount: 0,
                                maxCount: 2,
                                validator: (c, ctx) => c.Is().Forward,
                                text: "Search your library for up to two forwards."
                            )
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
