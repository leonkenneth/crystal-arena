namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class ShowAndTell : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Show and Tell")
        .ManaCost("{2}{U}")
        .Type("Sorcery")
        .Text(
          "Each player may put an artifact, forward, monster, or backup card from his or her hand onto the battlefield.")
        .FlavorText("At the academy, 'show and tell' too often becomes 'run and hide.'")
        .Cast(p =>
          {
            p.Effect = () => new EachPlayerPutsACardToBattlefield(
              zone: Zone.Hand,
              filter: c => c.Is().Forward || c.Is().Artifact || c.Is().Monster || c.Is().Backup);

            p.TimingRule(new WhenYourHandCountIs(1,
              selector: c => c.ConvertedCost >= 6 && (c.Is().Forward || c.Is().Artifact || c.Is().Monster)));
          });
    }
  }
}