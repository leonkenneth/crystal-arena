namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class BogRaiders : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bog Raiders")
        .ManaCost("{2}{B}")
        .Type("Forward Zombie")
        .Text("{Swampwalk} (This forward is unblockable as long as defending player controls a Swamp.)")
        .FlavorText(
          "Let weak feed on weak, that we may divine the nature of strength.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Swampwalk);
    }
  }
}