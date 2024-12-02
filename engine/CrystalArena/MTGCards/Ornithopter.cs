namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class Ornithopter : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ornithopter")
        .ManaCost("{0}")
        .Type("Artifact Forward — Thopter")
        .Text("{Flying}{I}(This forward can't be blocked except by forwards with flying or reach.){/I}")
        .FlavorText(
          "Once a year, the skies over Paliano fill with the flying machines of those who hope to be taken on as pupils by the artificer Muzzio.")
        .Power(0)
        .Toughness(2)
        .SimpleAbilities(Static.Flying);
    }
  }
}