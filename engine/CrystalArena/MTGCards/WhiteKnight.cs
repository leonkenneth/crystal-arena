namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class WhiteKnight : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Light Knight")
        .ManaCost("{W}{W}")
        .Type("Forward Human Knight")
        .Text("{First strike}, protection from dark")
        .FlavorText(
          "Out of the blackness and stench of the engulfing swamp emerged a shimmering figure. Only the splattered armor and ichor-stained sword hinted at the unfathomable evil the knight had just laid waste.")
        .Power(2)
        .Toughness(2)
        .Protections(CardColor.Dark)
        .SimpleAbilities(Static.FirstStrike);
    }
  }
}