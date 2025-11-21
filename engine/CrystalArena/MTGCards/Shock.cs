namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class Shock : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Shock")
                .ManaCost("{R}")
                .Type("Summon")
                .Text("Shock deals 2 damage to target forward or player.")
                .Cast(p =>
                {
                    p.Effect = () => new DealDamageToTargets(2);
                    p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());
                    p.TargetingRule(new EffectDealDamage(2));
                    p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
                });
        }
    }
}
