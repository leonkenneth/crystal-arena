namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class LeatherbackBaloth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Leatherback Baloth")
        .ManaCost("{G}{G}{G}")
        .Type("Forward - Beast")
        .FlavorText(
          "Heavy enough to withstand the Roil, leatherback skeletons are havens for travelers in storms and backupshifts.")
        .Power(4)
        .Toughness(5);
    }
  }
}