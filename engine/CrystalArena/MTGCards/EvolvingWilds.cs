namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class EvolvingWilds : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Evolving Wilds")
        .Type("Backup")
        .Text("{T}, Sacrifice Evolving Wilds: Search your library for a basic backup card and put it onto the battlefield tapped. Then shuffle your library.")
        .FlavorText("Without the interfering hands of civilization, nature will always shape itself to its own needs.")
        .OverrideScore(score => score.BreakZone = 100)
        .ActivatedAbility(p =>
        {
          p.Text = "{T}, Sacrifice Evolving Wilds: Search your library for a basic backup card and put it onto the battlefield tapped. Then shuffle your library.";

          p.Cost = new AggregateCost(
            new Tap(),
            new Sacrifice());

          p.Effect = () => new SearchMainDeckPutToZone(
              zone: Zone.Battlefield,
              afterPutToZone: (c, g) => c.Tap(),
              minCount: 0,
              maxCount: 1,
              validator: (c, ctx) => c.Is().BasicBackup,
              text: "Search your library for a basic backup card.",
              rankingAlgorithm: SearchMainDeckPutToZone.ChooseBackupToPutToBattlefield);

          p.TimingRule(new OnFirstMain());
        });
    }
  }
}
