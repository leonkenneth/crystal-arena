namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class ShimmeringBarrier : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Shimmering Barrier")
        .ManaCost("{1}{W}")
        .Type("Forward Wall")
        .Text("{Defender}, {First strike}{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Power(1)
        .Toughness(3)
        .Cycling("{2}")
        .SimpleAbilities(Static.Defender, Static.FirstStrike);
    }
  }
}