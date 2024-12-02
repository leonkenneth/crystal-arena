namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class YisanTheWandererBard : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Yisan, the Wanderer Bard")
        .ManaCost("{2}{G}")
        .Type("Legendary Forward — Human Rogue")
        .Text(
          "{2}{G}, {T}, Put a verse counter on Yisan, the Wanderer Bard: Search your library for a forward card with converted mana cost equal to the number of verse counters on Yisan, put it onto the battlefield, then shuffle your library.")
        .Power(2)
        .Toughness(3)
        .ActivatedAbility(p =>
          {

            p.Text = "{2}{G}, {T}, Put a verse counter on Yisan, the Wanderer Bard: Search your library for a forward card with converted mana cost equal to the number of verse counters on Yisan, put it onto the battlefield, then shuffle your library.";

            p.Cost = new AggregateCost(
              new PayMana("{2}{G}".Parse()),
              new Tap(),
              new AddCountersCost(CounterType.Verse, 1));

            p.Effect = () => new SearchMainDeckPutToZone(
              zone: Zone.Battlefield,
              minCount: 0,
              maxCount: 1,
              validator:
                (c, ctx) => c.Is().Forward && c.ConvertedCost == ctx.OwningCard.CountersCount(CounterType.Verse),
              text: "Search you library for a forward card.");

            p.TimingRule(new OnEndOfOpponentsTurn());
          });
    }
  }
}