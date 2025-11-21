namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Events;
    using CrystalArena.Triggers;

    public class Remembrance : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Remembrance")
                .ManaCost("{3}{W}")
                .Type("Monster")
                .Text(
                    "Whenever a nontoken forward you control dies, you may search your library for a card with the same name as that forward, reveal it, and put it into your hand. If you do, shuffle your library."
                )
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever a nontoken forward you control dies, you may search your library for a card with the same name as that forward, reveal it, and put it into your hand. If you do, shuffle your library.";

                    p.Trigger(
                        new OnZoneChanged(
                            @from: Zone.Battlefield,
                            to: Zone.BreakZone,
                            selector: (c, ctx) =>
                                ctx.You == c.Controller && c.Is().Forward && !c.Is().Token
                        )
                    );

                    p.Effect = () =>
                        new SearchMainDeckPutToZone(
                            zone: Zone.Hand,
                            minCount: 0,
                            maxCount: 1,
                            validator: (c, ctx) =>
                                ctx.TriggerMessage<ZoneChangedEvent>().Card.Name == c.Name
                        );
                });
        }
    }
}
