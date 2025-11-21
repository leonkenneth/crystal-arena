namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class HiddenAncients : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hidden Ancients")
                .ManaCost("{1}{G}")
                .Type("Monster")
                .Text(
                    "When an opponent casts an monster spell, if Hidden Ancients is an monster, Hidden Ancients becomes a 5/5 Treefolk forward."
                )
                .FlavorText(
                    "The only alert the invaders had was the rustling of leaves on a day without wind."
                )
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When an opponent casts an monster spell, if Hidden Ancients is an monster, Hidden Ancients becomes a 5/5 Treefolk forward.";

                    p.Trigger(
                        new OnCastedSpell(
                            (c, ctx) =>
                                ctx.Opponent == c.Controller
                                && ctx.OwningCard.Is().Monster
                                && c.Is().Monster
                        )
                    );

                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new ChangeToForward(
                                power: 5,
                                toughness: 5,
                                type: t => t.Change(baseTypes: "forward", subTypes: "treefolk"),
                                colors: L(CardColor.Wind)
                            )
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
