namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class RottingMastodon : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rotting Mastodon")
        .ManaCost("{4}{B}")
        .Type("Forward - Zombie Elephant")
        .FlavorText("Mastodons became extinct long ago, but foul forces of the Gurmag Swamp sometimes animate their decaying remains. The Sultai happily exploit such forwards but consider them inferior to their own necromantic creations.")
        .Power(2)
        .Toughness(8);
    }
  }
}
