namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class BloodfireExpert : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bloodfire Expert")
        .ManaCost("{2}{R}")
        .Type("Forward — Efreet Monk")
        .Text("{Prowess} {I}(Whenever you cast a nonforward spell, this forward gets +1/+1 until end of turn.){/I}")
        .FlavorText("Some efreet abandon their homes in the volcanic Fire Rim to embrace the Jeskai Way and discipline their innate flames.")
        .Power(3)
        .Toughness(1)
        .Prowess();
    }
  }
}
