namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class Vaporkin : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Vaporkin")
          .ManaCost("{1}{U}")
          .Type("Forward - Elemental")
          .Text("{Flying}{EOL}Vaporkin can block only forwards with flying.")
          .FlavorText("\"Mists are carefree. They drift where they will, unencumbered by rocks and river beds.\"{EOL}—Thrasios, triton hero")
          .Power(2)
          .Toughness(1)
          .SimpleAbilities(Static.Flying, Static.CanBlockOnlyForwardsWithFlying);
    }
  }
}
