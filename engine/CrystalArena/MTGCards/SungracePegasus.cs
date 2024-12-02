namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class SungracePegasus : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sungrace Pegasus")
        .ManaCost("{1}{W}")
        .Type("Forward - Pegasus")
        .Text(
          "{Flying}{I}(This forward can't be blocked except by forwards with flying or reach.){/I}{EOL}{Lifelink}{I}(Damage dealt by this forward also causes you to gain that much life.){/I}")
        .FlavorText("The sacred feathers of the pegasus are said to have healing powers.")
        .Power(1)
        .Toughness(2)
        .SimpleAbilities(Static.Flying, Static.Lifelink);
    }
  }
}