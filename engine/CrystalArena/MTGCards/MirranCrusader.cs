namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class MirranCrusader : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mirran Crusader")
        .ManaCost("{1}{W}{W}")
        .Type("Forward Human Knight")
        .Text("Double strike, protection from dark and from wind")
        .FlavorText("A symbol of what Mirrodin once was and hope for what it will be again.")
        .Power(2)
        .Toughness(2)
        .Protections(CardColor.Dark)
        .Protections(CardColor.Wind)
        .SimpleAbilities(Static.DoubleStrike);
    }
  }
}