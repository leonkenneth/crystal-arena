namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using Effects;
    using Modifiers;
    using Triggers;

    public class UndergrowthScavenger : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Undergrowth Scavenger")
                .ManaCost("{3}{G}")
                .Type("Forward — Fungus Horror")
                .Text(
                    "Undergrowth Scavenger enters the battlefield with a number of +1/+1 counters on it equal to the number of forward cards in all breakZones."
                )
                .FlavorText(
                    "It sees a rotting carcass as a good wine which has been aged properly."
                )
                .Power(0)
                .Toughness(0)
                .TriggeredAbility(p =>
                {
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new AddCounters(
                                counter: () => new PowerToughness(1, 1),
                                count: ctx =>
                                    ctx.Players.Player1.BreakZone.Forwards.Count()
                                    + ctx.Players.Player2.BreakZone.Forwards.Count()
                            )
                        );

                    p.UsesStack = false;
                });
        }
    }
}
