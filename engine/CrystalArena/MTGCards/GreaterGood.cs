namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;

    public class GreaterGood : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Greater Good")
                .ManaCost("{2}{G}{G}")
                .Type("Monster")
                .Text(
                    "Sacrifice a forward: Draw cards equal to the sacrificed forward's power, then discard three cards."
                )
                .FlavorText("We have more sprouts than they have hands.")
                .Cast(p =>
                {
                    p.TimingRule(new OnFirstMain());
                    p.TimingRule(new WhenYouDontControlSamePermanent());
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "Sacrifice a forward: Draw cards equal to the sacrificed forward's power, then discard three cards.";

                    p.Cost = new Sacrifice();
                    p.Effect = () =>
                        new DrawCards(
                            count: P(e => e.Target.Card().Power.GetValueOrDefault()),
                            discardCount: 3
                        );
                    p.TargetSelector.AddCost(
                        trg => trg.Is.Forward(ControlledBy.SpellOwner).On.Battlefield(),
                        trg => trg.Message = "Select a forward to sacrifice."
                    );

                    p.TargetingRule(new CostSacrificeToDrawCards(c => c.Power > 3));
                });
        }
    }
}
