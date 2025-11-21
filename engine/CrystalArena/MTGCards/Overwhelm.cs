namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class Overwhelm : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Overwhelm")
                .ManaCost("{5}{G}{G}")
                .Type("Sorcery")
                .Text(
                    "{Convoke}{I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}Forwards you control get +3/+3 until end of turn."
                )
                .FlavorText(
                    "The Conclave acts with a single will, expressed by countless warriors."
                )
                .SimpleAbilities(Static.Convoke)
                .Cast(p =>
                {
                    p.Effect = () =>
                        new ApplyModifiersToPermanents(
                            selector: (c, ctx) => c.Is().Forward && ctx.You == c.Controller,
                            modifier: () => new AddPowerAndToughness(3, 3) { UntilEot = true }
                        ).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

                    p.TimingRule(new OnFirstMain());
                });
        }
    }
}
