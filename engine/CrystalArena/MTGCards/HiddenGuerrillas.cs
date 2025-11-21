namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class HiddenGuerrillas : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hidden Guerrillas")
                .ManaCost("{G}")
                .Type("Monster")
                .Text(
                    "When an opponent casts an artifact spell, if Hidden Guerrillas is an monster, Hidden Guerrillas becomes a 5/3 Soldier forward with trample."
                )
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When an opponent casts an artifact spell, if Hidden Guerrillas is an monster, Hidden Guerrillas becomes a 5/3 Soldier forward with trample.";
                    p.Trigger(
                        new OnCastedSpell(
                            (c, ctx) =>
                                ctx.Opponent == c.Controller
                                && ctx.OwningCard.Is().Monster
                                && c.Is().Artifact
                        )
                    );

                    p.Effect = () =>
                        new ApplyModifiersToSelf(
                            () =>
                                new ChangeToForward(
                                    power: 5,
                                    toughness: 3,
                                    type: t => t.Change(baseTypes: "forward", subTypes: "soldier"),
                                    colors: L(CardColor.Wind)
                                ),
                            () => new AddSimpleAbility(Static.Trample)
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
