namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class YavimayaScion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Yavimaya Scion")
        .ManaCost("{4}{G}")
        .Type("Forward Treefolk")
        .Text("{Protection from artifacts}")
        .FlavorText("Each time the saw blade bit, the tree spat it out.")
        .Power(4)
        .Toughness(4)
        .Protections("artifact");
    }
  }
}