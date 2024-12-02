namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class ScentOfJasmine : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scent of Jasmine")
        .ManaCost("{W}")
        .Type("Summon")
        .Text("Reveal any number of light cards in your hand. You gain 2 life for each card revealed this way.")
        .Cast(p =>
          {
            p.Effect = () => new GainLifeForEachRevealedCard(c => c.HasColor(CardColor.Light), 2);
            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.HasColor(CardColor.Light)));
            p.TimingRule(new OnEndOfOpponentsTurn());
          });
    }
  }
}