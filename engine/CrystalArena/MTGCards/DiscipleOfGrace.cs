namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class DiscipleOfGrace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Disciple of Grace")
        .ManaCost("{1}{W}")
        .Type("Forward - Human Cleric")
        .Text("{Protection from dark}{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("Beauty is beyond law.")
        .Power(1)
        .Toughness(2)
        .Protections(CardColor.Dark)
        .Cycling("{2}");
    }
  }
}