namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class SquirmingMass : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Squirming Mass")
        .ManaCost("{1}{B}")
        .Type("Forward Horror")
        .Text("{Fear} (This forward can't be blocked except by artifact forwards and/or dark forwards.)")
        .FlavorText("Only the coldest hearts and the strongest stomachs can stand against it.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Fear);
    }
  }
}