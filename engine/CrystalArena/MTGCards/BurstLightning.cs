namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;

    public class BurstLightning : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Burst Lightning")
                .ManaCost("{R}")
                .Type("Summon")
                .Text(
                    "{Kicker} {4}{EOL}Burst Lightning deals 2 damage to target forward or player. If Burst Lightning was kicked, it deals 4 damage to that forward or player instead."
                )
                .Cast(p =>
                {
                    p.Effect = () => new DealDamageToTargets(2);
                    p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());

                    p.TargetingRule(new EffectDealDamage(2));
                    p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
                })
                .Cast(p =>
                {
                    p.Text = p.KickerDescription;
                    p.Cost = new PayMana("{4}{R}".Parse());
                    p.Effect = () => new DealDamageToTargets(4);
                    p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());

                    p.TargetingRule(new EffectDealDamage(4));
                    p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
                });
        }
    }
}
