namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using Effects;
    using Modifiers;

    public class EndlessObedience : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Endless Obedience")
                .ManaCost("{4}{B}{B}")
                .Type("Sorcery")
                .Text(
                    "{Convoke} {I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}Put target forward card from a breakZone onto the battlefield under your control."
                )
                .FlavorText("The death of a scout can be as informative as a safe return.")
                .SimpleAbilities(Static.Convoke)
                .Cast(p =>
                {
                    p.Text =
                        "Put target forward card from a breakZone onto the battlefield under your control.";

                    p.Effect = () =>
                        new CompoundEffect(
                            new ApplyModifiersToTargets(() =>
                                new ChangeController(m => m.SourceCard.Controller)
                            )
                            {
                                ShouldResolve = ctx => ctx.Opponent == ctx.Target.Controller(),
                            },
                            new PutTargetsToBattlefield()
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().In.BreakZone());

                    p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
                });
        }
    }
}
