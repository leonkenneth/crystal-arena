namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;

    public class CarrionBeetles : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Carrion Beetles")
                .ManaCost("{B}")
                .Type("Forward Insect")
                .Text(
                    "{2}{B},{T}: RemoveFromPlay up to three target cards from a single breakZone."
                )
                .FlavorText("It's all fun and games until someone loses an eye.")
                .Power(1)
                .Toughness(1)
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{2}{B},{T}: RemoveFromPlay up to three target cards from a single breakZone.";
                    p.Cost = new AggregateCost(new PayMana("{2}{B}".Parse()), new Tap());
                    p.Effect = () => new RemoveFromPlayTargets();
                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Card().In.BreakZone(),
                        trg =>
                        {
                            trg.MinCount = 0;
                            trg.MaxCount = 3;
                        }
                    );
                    p.TimingRule(
                        new Any(
                            new WhenTopSpellTargetsCardInBreakZone(),
                            new OnEndOfOpponentsTurn()
                        )
                    );
                    p.TargetingRule(new EffectOrCostRankBy(c => -c.Score, ControlledBy.Opponent));
                });
        }
    }
}
