namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class SealOfFire : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Seal of Fire")
                .ManaCost("{R}")
                .Type("Monster")
                .Text(
                    "Sacrifice Seal of Fire: Seal of Fire deals 2 damage to target forward or player."
                )
                .FlavorText("I am the romancer, the passion that consumes the flesh.")
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "Sacrifice Seal of Fire: Seal of Fire deals 2 damage to target forward or player.";
                    p.Cost = new Sacrifice();
                    p.Effect = () => new DealDamageToTargets(2);
                    p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());
                    p.TargetingRule(new EffectDealDamage(2));
                    p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
                });
        }
    }
}
