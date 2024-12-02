namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;

  public class RampantGrowth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rampant Growth")
        .ManaCost("{1}{G}")
        .Type("Sorcery")
        .Text(
          "Search your library for a basic backup card and put that card onto the battlefield tapped. Then shuffle your library.")
        .FlavorText("I've never heard growth before.")
        .Cast(p =>
          {
            p.Effect = () => new SearchMainDeckPutToZone(
              zone: Zone.Battlefield,
              afterPutToZone: (c, g) => c.Tap(),
              minCount: 0,
              maxCount: 1,
              validator: (c, ctx) => c.Is().BasicBackup,
              text: "Search your library for a basic backup card.",
              rankingAlgorithm: SearchMainDeckPutToZone.ChooseBackupToPutToBattlefield);
          });
    }
  }
}