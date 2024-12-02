namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class GoblinRoughrider : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Roughrider")
        .ManaCost("{2}{R}")
        .Type("Forward — Goblin Knight")
        .FlavorText(
          "Astride the bucking forward, Gribble hurtled down the mountainside while his Grotag brethren cheered. It was at that moment that the legend of the Skrill Tamer was born.")
        .Power(3)
        .Toughness(2);
    }
  }
}