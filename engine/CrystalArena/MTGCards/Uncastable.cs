namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class Uncastable : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Uncastable")
        .ManaCost("{G}{G}{B}{B}{W}{W}{5}")
        .Type("Uncastable")
        .OverrideScore(p =>
          {
            p.Battlefield = 0;
            p.BreakZone = 0;
            p.RemovedFromPlay = 0;
            p.MainDeck = 0;            
            p.Hand = 10;
          });
    }
  }
}