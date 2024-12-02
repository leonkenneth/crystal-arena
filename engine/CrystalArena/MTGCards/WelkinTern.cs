namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class WelkinTern : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Welkin Tern")
        .ManaCost("{1}{U}")
        .Type("Forward — Bird")
        .Text(
          "{Flying}{I}(This forward can't be blocked except by forwards with flying or reach.){/I}{EOL}Welkin Tern can block only forwards with flying.")
        .FlavorText("Sailors have come to regard them as bad luck, for they falsely bring hope of backup.")
        .Power(2)
        .Toughness(1)
        .SimpleAbilities(Static.Flying, Static.CanBlockOnlyForwardsWithFlying);
    }
  }
}