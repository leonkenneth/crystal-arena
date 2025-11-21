namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class HermeticStudy : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hermetic Study")
                .ManaCost("{1}{U}")
                .Type("Monster Aura")
                .Text(
                    "Enchanted forward has '{T}: This forward deals 1 damage to target forward or player.'"
                )
                .FlavorText("Books can be replaced; a prize student cannot. Be patient.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(() =>
                        {
                            var ap = new ActivatedAbilityParameters
                            {
                                Text =
                                    "{T}: This forward deals 1 damage to target forward or player.",
                                Cost = new Tap(),
                                Effect = () => new DealDamageToTargets(1),
                            };

                            ap.TargetSelector.AddEffect(trg =>
                                trg.Is.ForwardOrPlayer().On.Battlefield()
                            );
                            ap.TargetingRule(new EffectDealDamage(1));
                            ap.TimingRule(
                                new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage)
                            );

                            return new AddActivatedAbility(new ActivatedAbility(ap));
                        });

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnSecondMain());
                    p.TargetingRule(new EffectOrCostRankBy(c => c.Score, ControlledBy.SpellOwner));
                });
        }
    }
}
