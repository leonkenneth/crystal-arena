namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class CrowdsFavor : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Crowd's Favor")
                .ManaCost("{R}")
                .Type("Summon")
                .Text(
                    "{Convoke} {I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}Target forward gets +1/+0 and gains first strike until end of turn. {I}(It deals combat damage before forwards without first strike.){/I}"
                )
                .SimpleAbilities(Static.Convoke)
                .Cast(p =>
                {
                    p.Text = "Target forward gets +1/+0 and gains first strike until end of turn.";

                    p.Effect = () =>
                        new ApplyModifiersToTargets(
                            () => new AddPowerAndToughness(1, 0) { UntilEot = true },
                            () => new AddSimpleAbility(Static.FirstStrike) { UntilEot = true }
                        ).SetTags(EffectTag.IncreasePower);

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(
                        new Any(
                            new BeforeYouDeclareAttackers(),
                            new AfterOpponentDeclaresAttackers()
                        )
                    );
                    p.TargetingRule(new EffectPumpSummon(1, 0));
                });
        }
    }
}
