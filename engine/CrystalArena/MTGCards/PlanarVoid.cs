namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Events;
    using CrystalArena.Triggers;

    public class PlanarVoid : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Planar Void")
                .ManaCost("{B}")
                .Type("Monster")
                .Text(
                    "Whenever another card is put into a breakZone from anywhere, exile that card."
                )
                .FlavorText(
                    "'Planeswalking isn't about walking. It's about falling and screaming.'"
                )
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever another card is put into a breakZone from anywhere, exile that card.";

                    p.Trigger(
                        new OnZoneChanged(
                            to: Zone.BreakZone,
                            selector: delegate
                            {
                                return true;
                            }
                        )
                    );

                    p.Effect = () =>
                        new RemoveFromPlayCard(
                            P(e => e.TriggerMessage<ZoneChangedEvent>().Card),
                            Zone.BreakZone
                        );
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
