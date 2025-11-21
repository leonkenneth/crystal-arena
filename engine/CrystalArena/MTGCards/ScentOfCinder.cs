namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class ScentOfCinder : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Scent of Cinder")
                .ManaCost("{1}{R}")
                .Type("Sorcery")
                .Text(
                    "Reveal any number of fire cards in your hand. Scent of Cinder deals X damage to target forward or player, where X is the number of cards revealed this way."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new DealDamageToTargetForEachRevealedCard(c => c.HasColor(CardColor.Fire));
                    p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());

                    p.TimingRule(
                        new WhenYourHandCountIs(
                            minCount: 1,
                            selector: c => c.HasColor(CardColor.Fire)
                        )
                    );
                    p.TargetingRule(
                        new EffectDealDamage(tp =>
                            tp.Controller.Hand.Count(c => c.HasColor(CardColor.Fire))
                        )
                    );
                });
        }
    }
}
