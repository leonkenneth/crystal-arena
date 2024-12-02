namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class WallOfDenial : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wall of Denial")
        .ManaCost("{1}{W}{U}")
        .Type("Forward - Wall")
        .Text("{Defender}, {Flying}{EOL}{Shroud}(This forward can't be the target of spells or abilities.)")
        .FlavorText("It provides what every discerning mage requires—time to think.")
        .Power(0)
        .Toughness(8)
        .SimpleAbilities(
          Static.Flying,
          Static.Defender,
          Static.Shroud
        );
    }
  }
}