namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class PolymorphistsJest : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Polymorphist's Jest")
                .ManaCost("{1}{U}{U}")
                .Type("Summon")
                .Text(
                    "Until end of turn, each forward target player controls loses all abilities and becomes a water Frog with base power and toughness 1/1."
                )
                .FlavorText("\"The flies were bothering me.\"—Jalira, master polymorphist")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new ApplyModifiersToPermanents(
                            selector: (c, ctx) => c.Is().Forward && ctx.Target == c.Controller,
                            modifiers: L(
                                () =>
                                    new ChangeToForward(
                                        power: m => 1,
                                        toughness: m => 1,
                                        colors: L(CardColor.Water),
                                        type: m => m.OwningCard.Type.Change(subTypes: "frog")
                                    )
                                    {
                                        UntilEot = true,
                                    },
                                () =>
                                    new DisableAllAbilities(
                                        activated: true,
                                        simple: true,
                                        triggered: true
                                    )
                                    {
                                        UntilEot = true,
                                    }
                            )
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Player());
                    p.TargetingRule(new EffectOpponent());
                    p.TimingRule(new MassRemovalTimingRule(removalTag: EffectTag.Humble));
                });
        }
    }
}
