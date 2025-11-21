namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class OpalCaryatid : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Opal Caryatid")
                .ManaCost("{W}")
                .Type("Monster")
                .Text(
                    "When an opponent casts a forward spell, if Opal Caryatid is an monster, Opal Caryatid becomes a 2/2 Soldier forward."
                )
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When an opponent casts a forward spell, if Opal Caryatid is an monster, Opal Caryatid becomes a 2/2 Soldier forward.";
                    p.Trigger(
                        new OnCastedSpell(
                            (c, ctx) =>
                                ctx.Opponent == c.Controller
                                && ctx.OwningCard.Is().Monster
                                && c.Is().Forward
                        )
                    );
                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new ChangeToForward(
                                power: 2,
                                toughness: 2,
                                type: t => t.Change(baseTypes: "forward", subTypes: "soldier"),
                                colors: L(CardColor.Light)
                            )
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
