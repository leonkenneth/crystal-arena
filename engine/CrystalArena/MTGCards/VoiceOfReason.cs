namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class VoiceOfReason : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Voice of Reason")
        .ManaCost("{3}{W}")
        .Type("Forward Angel")
        .Text("{Flying}, protection from water")
        .FlavorText(
          "Next to Grace is Reason, and Reason must be retained. If the web of Reason comes unwoven, madness will escape.")
        .Power(2)
        .Toughness(2)
        .Protections(CardColor.Water)
        .SimpleAbilities(Static.Flying);
    }
  }
}