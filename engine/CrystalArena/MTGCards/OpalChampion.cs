namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class OpalChampion : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Opal Champion")
                .ManaCost("{2}{W}")
                .Type("Monster")
                .Text(
                    "When an opponent casts a forward spell, if Opal Champion is an monster, Opal Champion becomes a 3/3 Knight forward with first strike."
                )
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When an opponent casts a forward spell, if Opal Champion is an monster, Opal Champion becomes a 3/3 Knight forward with first strike.";
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
                                    power: 3,
                                    toughness: 3,
                                    type: t => t.Change(baseTypes: "forward", subTypes: "knight"),
                                    colors: L(CardColor.Light)
                                ),
                            () => new AddSimpleAbility(Static.FirstStrike)
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
