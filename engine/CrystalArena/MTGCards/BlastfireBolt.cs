namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using Effects;

    public class BlastfireBolt : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Blastfire Bolt")
                .ManaCost("{5}{R}")
                .Type("Summon")
                .Text(
                    "Blastfire Bolt deals 5 damage to target forward. Destroy all Equipment attached to that forward."
                )
                .FlavorText(
                    "\"Encase yourself in the most elaborate armor, and cower behind the heaviest shield. I would hate for you to feel helpless.\"{EOL}—Korig the Ruiner"
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CompoundEffect(
                            new DealDamageToTargets(5),
                            new DestroyAttachedAttachments(
                                P(e => e.Target.Card().Cards),
                                (c, ctx) => c.Is().Equipment
                            )
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TargetingRule(new EffectDealDamage(5));
                });
        }
    }
}
