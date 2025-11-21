namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class PlanarCollapse : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Planar Collapse")
                .ManaCost("{1}{W}")
                .Type("Monster")
                .Text(
                    "At the beginning of your upkeep, if there are four or more forwards on the battlefield, sacrifice Planar Collapse and destroy all forwards. They can't be regenerated."
                )
                .FlavorText(
                    "With heavy heart, Urza doused one world's light to rekindle another's."
                )
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of your upkeep, if there are four or more forwards on the battlefield, sacrifice Planar Collapse and destroy all forwards. They can't be regenerated.";

                    p.Trigger(
                        new OnStepStart(Step.Upkeep)
                        {
                            Condition = ctx =>
                                ctx.Players.Permanents().Count(c => c.Is().Forward) >= 4,
                        }
                    );

                    p.Effect = () =>
                        new CompoundEffect(
                            new SacrificeOwner(),
                            new DestroyAllPermanents(
                                (c, ctx) => c.Is().Forward,
                                allowToRegenerate: false
                            )
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
