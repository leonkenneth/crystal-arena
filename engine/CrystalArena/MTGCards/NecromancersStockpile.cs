namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;

    public class NecromancersStockpile : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Necromancer's Stockpile")
                .ManaCost("{1}{B}")
                .Type("Monster")
                .Text(
                    "{1}{B}, Discard a forward card: Draw a card. If the discarded card was a Zombie card, put a 2/2 dark Zombie forward token onto the battlefield tapped."
                )
                .FlavorText("The experiments decided to perform some research of their own.")
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{1}{B}, Discard a forward card: Draw a card. If the discarded card was a Zombie card, put a 2/2 dark Zombie forward token onto the battlefield tapped.";

                    p.Cost = new AggregateCost(new PayMana("{1}{B}".Parse()), new DiscardTarget());

                    p.Effect = () =>
                        new CompoundEffect(
                            new DrawCards(1),
                            new CreateTokens(
                                count: 1,
                                token: Card.Named("Zombie")
                                    .Power(2)
                                    .Toughness(2)
                                    .Type("Token Forward - Zombie")
                                    .Colors(CardColor.Dark),
                                afterTokenComesToPlay: (token, game) => token.Tap()
                            )
                            {
                                ShouldResolve = ctx => ctx.Target.Card().Is("zombie"),
                            }
                        );

                    p.TargetSelector.AddCost(trg => trg.Is.Forward().In.OwnersHand());
                    p.TargetingRule(new CostDiscardCard(c => c.Is("zombie") ? -1 : c.Score));
                    p.TimingRule(
                        new Any(new DefaultCyclingTimingRule(), new OnEndOfOpponentsTurn())
                    );
                });
        }
    }
}
