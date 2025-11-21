namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class Pyromancy : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Pyromancy")
                .ManaCost("{2}{R}{R}")
                .Type("Monster")
                .Text(
                    "{3}, Discard a card at random: Pyromancy deals damage to target forward or player equal to the converted mana cost of the discarded card."
                )
                .FlavorText("Who harnesses fire controls the world.")
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{3}, Discard a card at random: Pyromancy deals damage to target forward or player equal to the converted mana cost of the discarded card.";

                    p.Cost = new AggregateCost(new PayMana(3.Colorless()), new DiscardRandom());

                    p.Effect = () =>
                        new DealDamageToTargets(
                            amount: P(
                                e => e.Targets.Cost.First().Card().ConvertedCost,
                                EvaluateAt.AfterCost
                            )
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());

                    p.TimingRule(
                        new WhenYourHandCountIs(minCount: 1, selector: c => c.ConvertedCost > 0)
                    );
                    p.TargetingRule(
                        new EffectDealDamage(getAmount: tp =>
                            tp.Controller.Hand.Min(c => c.ConvertedCost)
                        )
                    );
                    p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
                });
        }
    }
}
