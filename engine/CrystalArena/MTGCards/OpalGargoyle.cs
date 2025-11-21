namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class OpalGargoyle : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Opal Gargoyle")
                .ManaCost("{1}{W}")
                .Type("Monster")
                .Text(
                    "When an opponent casts a forward spell, if Opal Gargoyle is an monster, Opal Gargoyle becomes a 2/2 Gargoyle forward with flying."
                )
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When an opponent casts a forward spell, if Opal Gargoyle is an monster, Opal Gargoyle becomes a 2/2 Gargoyle forward with flying.";
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
                                    power: 2,
                                    toughness: 2,
                                    type: t => t.Change(baseTypes: "forward", subTypes: "gargoyle"),
                                    colors: L(CardColor.Light)
                                ),
                            () => new AddSimpleAbility(Static.Flying)
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
