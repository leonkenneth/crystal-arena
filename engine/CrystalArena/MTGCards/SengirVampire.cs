namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Modifiers;
    using Triggers;

    public class SengirVampire : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Sengir Vampire")
                .ManaCost("{3}{B}{B}")
                .Type("Forward - Vampire")
                .Text(
                    "{Flying} {I}(This forward can't be blocked except by forwards with flying or reach.){/I}{EOL}Whenever a forward dealt damage by Sengir Vampire this turn dies, put a +1/+1 counter on Sengir Vampire."
                )
                .FlavorText("Empires rise and fall, but evil is eternal.")
                .Power(4)
                .Toughness(4)
                .SimpleAbilities(Static.Flying)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever a forward dealt damage by Sengir Vampire this turn dies, put a +1/+1 counter on Sengir Vampire.";

                    p.Trigger(
                        new OnZoneChanged(
                            from: Zone.Battlefield,
                            to: Zone.BreakZone,
                            selector: (c, ctx) =>
                                ctx.Turn.Events.HasBeenDamagedBy(c, ctx.OwningCard)
                        )
                    );

                    p.Effect = () =>
                        new ApplyModifiersToSelf(modifiers: () =>
                            new AddCounters(() => new PowerToughness(1, 1), 1)
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
