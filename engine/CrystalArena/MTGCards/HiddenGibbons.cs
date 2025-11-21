namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class HiddenGibbons : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hidden Gibbons")
                .ManaCost("{G}")
                .Type("Monster")
                .Text(
                    "When an opponent casts an summon spell, if Hidden Gibbons is an monster, Hidden Gibbons becomes a 4/4 Ape forward."
                )
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When an opponent casts an summon spell, if Hidden Gibbons is an monster, Hidden Gibbons becomes a 4/4 Ape forward.";
                    p.Trigger(
                        new OnCastedSpell(
                            (c, ctx) =>
                                ctx.Opponent == c.Controller
                                && ctx.OwningCard.Is().Monster
                                && c.Is().Summon
                        )
                    );

                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new ChangeToForward(
                                power: 4,
                                toughness: 4,
                                type: t => t.Change(baseTypes: "forward", subTypes: "ape"),
                                colors: L(CardColor.Wind)
                            )
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
