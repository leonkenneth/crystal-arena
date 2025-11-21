namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class JeskaiCharm : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Jeskai Charm")
                .ManaCost("{U}{R}{W}")
                .Type("Summon")
                .Text(
                    "Choose one —{EOL}• Put target forward on top of its owner's library.{EOL}• Jeskai Charm deals 4 damage to target opponent.{EOL}• Forwards you control get +1/+1 and gain lifelink until end of turn."
                )
                .Cast(p =>
                {
                    p.Text = "{{U}}{{R}}{{W}}: Put target forward on top of its owner's library.";
                    p.Effect = () => new PutTargetsOnTopOfMainDeck();
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TargetingRule(new EffectPutOnTopOfMainDeck());
                    p.TimingRule(
                        new TargetRemovalTimingRule().RemovalTags(
                            EffectTag.Bounce,
                            EffectTag.ForwardsOnly
                        )
                    );
                })
                .Cast(p =>
                {
                    p.Text = "{{U}}{{R}}{{W}}: Jeskai Charm deals 4 damage to target opponent.";
                    p.Effect = () => new DealDamageToTargets(4);
                    p.TargetSelector.AddEffect(trg => trg.Is.Opponent());
                    p.TargetingRule(new EffectDealDamage(4));
                    p.TimingRule(new OnSecondMain());
                })
                .Cast(p =>
                {
                    p.Text =
                        "{{U}}{{R}}{{W}}: Forwards you control get +1/+1 and gain lifelink until end of turn.";
                    p.Effect = () =>
                        new ApplyModifiersToPermanents(
                            selector: (c, ctx) => c.Is().Forward && ctx.You == c.Controller,
                            modifiers: L(
                                () => new AddPowerAndToughness(1, 1) { UntilEot = true },
                                () => new AddSimpleAbility(Static.Lifelink) { UntilEot = true }
                            )
                        );

                    p.TimingRule(
                        new Any(
                            new AfterOpponentDeclaresBlockers(),
                            new AfterOpponentDeclaresAttackers()
                        )
                    );
                });
        }
    }
}
