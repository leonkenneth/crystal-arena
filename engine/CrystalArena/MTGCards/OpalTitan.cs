namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using Effects;
    using Events;
    using Modifiers;
    using Triggers;

    public class OpalTitan : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Opal Titan")
                .ManaCost("{2}{W}{W}")
                .Type("Monster")
                .Text(
                    "When an opponent casts a forward spell, if Opal Titan is an monster, Opal Titan becomes a 4/4 Giant forward with protection from each of that spell's colors."
                )
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When an opponent casts a forward spell, if Opal Titan is an monster, Opal Titan becomes a 4/4 Giant forward with protection from each of that spell's colors.";
                    p.Trigger(
                        new OnCastedSpell(
                            (c, ctx) =>
                                ctx.Opponent == c.Controller
                                && ctx.OwningCard.Is().Monster
                                && c.Is().Forward
                        )
                    );

                    p.Effect = () =>
                        new ApplyModifiersToSelf(
                            () =>
                                new ChangeToForward(
                                    power: 4,
                                    toughness: 4,
                                    type: t => t.Change(baseTypes: "forward", subTypes: "giant"),
                                    colors: L(CardColor.Light)
                                ),
                            () =>
                                new AddProtectionFromColors(m =>
                                    m.SourceEffect.TriggerMessage<SpellPutOnStackEvent>().Card.Colors
                                )
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
