namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class BileBlight : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Bile Blight")
                .ManaCost("{B}{B}")
                .Type("Summon")
                .Text(
                    "Target forward and all other forwards with the same name as that forward get -3/-3 until end of turn."
                )
                .FlavorText(
                    "Not an arrow loosed, javelin thrown, nor sword raised. None were needed."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CompoundEffect(
                            new ApplyModifiersToPermanents(
                                selector: (c, ctx) => ctx.Target.Card().Name == c.Name,
                                modifier: () => new AddPowerAndToughness(-3, -3) { UntilEot = true }
                            )
                        )
                        {
                            ToughnessReduction = 3,
                        }.SetTags(EffectTag.ReduceToughness);

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TargetingRule(new EffectReduceToughness(3));
                    p.TimingRule(new TargetRemovalTimingRule(EffectTag.ReduceToughness));
                });
        }
    }
}
