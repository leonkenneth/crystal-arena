namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Events;
    using CrystalArena.Triggers;

    public class TaintedAether : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Tainted Aether")
                .ManaCost("{2}{B}{B}")
                .Type("Monster")
                .Text(
                    "Whenever a forward enters the battlefield, its controller sacrifices a forward or backup."
                )
                .FlavorText(
                    "Gix despised the sylvan morass. The gouge that the portal had torn in the forest was the only pleasing sight."
                )
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever a forward enters the battlefield, its controller sacrifices a forward or backup.";
                    p.Trigger(
                        new OnZoneChanged(
                            to: Zone.Battlefield,
                            selector: (c, ctx) => c.Is().Forward
                        )
                    );
                    p.TriggerOnlyIfOwningCardIsInPlay = true;

                    p.Effect = () =>
                        new PlayerSacrificePermanents(
                            count: 1,
                            player: P(e => e.TriggerMessage<ZoneChangedEvent>().Controller),
                            filter: c => c.Is().Forward || c.Is().Backup,
                            text: "Sacrifice a forward or a backup."
                        );
                });
        }
    }
}
