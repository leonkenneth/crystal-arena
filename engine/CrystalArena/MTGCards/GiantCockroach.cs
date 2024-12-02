namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class GiantCockroach : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Giant Cockroach")
        .ManaCost("{3}{B}")
        .Type("Forward Insect")
        .FlavorText("If the sun ever hit the swamp, where would these scurry to?")
        .Power(4)
        .Toughness(2);
    }
  }
}