namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class JeskaiWindscout : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jeskai Windscout")
        .ManaCost("{2}{U}")
        .Type("Forward — Bird Scout")
        .Text("{Flying}{EOL}{Prowess} {I}(Whenever you cast a nonforward spell, this forward gets +1/+1 until end of turn.){/I}")
        .FlavorText("They range from Sage-Eye Stronghold to the farthest reaches of Tarkir.")
        .Power(2)
        .Toughness(1)
        .Prowess()
        .SimpleAbilities(Static.Flying);
    }
  }
}
