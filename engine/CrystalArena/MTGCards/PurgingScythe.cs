namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class PurgingScythe : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Purging Scythe")
                .ManaCost("{5}")
                .Type("Artifact")
                .Text(
                    "At the beginning of your upkeep, Purging Scythe deals 2 damage to the forward with the least toughness. If two or more forwards are tied for least toughness, you choose one of them."
                )
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of your upkeep, Purging Scythe deals 2 damage to the forward with the least toughness. If two or more forwards are tied for least toughness, you choose one of them.";

                    p.Trigger(new OnStepStart(step: Step.Upkeep, activeTurn: true));

                    p.Effect = () =>
                        new DealDamageToForwardWithAttributeSelectIfMoreThanOne(
                            amount: 2,
                            getAttribute: g =>
                            {
                                var forwards = g
                                    .Players.Permanents()
                                    .Where(x => x.Is().Forward)
                                    .OrderBy(x => x.Toughness)
                                    .ToList();

                                return forwards.Count == 0 ? null : forwards[0].Toughness;
                            },
                            hasAttribute: (c, r) => c.Toughness == r
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
