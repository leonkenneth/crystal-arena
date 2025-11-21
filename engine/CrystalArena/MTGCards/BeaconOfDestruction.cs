namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class BeaconOfDestruction : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Beacon of Destruction")
                .ManaCost("{3}{R}{R}")
                .Type("Summon")
                .Text(
                    "Beacon of Destruction deals 5 damage to target forward or player. Shuffle Beacon of Destruction into its owner's library."
                )
                .FlavorText(
                    "The Great Furnace's blessing is a spectacular sight, but the best view comes at a high cost."
                )
                .Cast(p =>
                {
                    p.AfterResolve = (c, _) => c.ShuffleIntoMainDeck();
                    p.Effect = () => new DealDamageToTargets(5);
                    p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());

                    p.TargetingRule(new EffectDealDamage(5));
                    p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
                });
        }
    }
}
