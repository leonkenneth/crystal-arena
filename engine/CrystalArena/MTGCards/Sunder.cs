namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class Sunder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sunder")
        .ManaCost("{3}{U}{U}")
        .Type("Summon")
        .Text("Return all backups to their owners' hands.")
        .FlavorText(
          "The flow of time was disrupted; like a flooding river it rose from its banks. Tolaria was drowned in an summon that stretched toward infinity.")
        .OverrideScore(p => p.Hand = 50)
        .Cast(p =>
          {
            p.Effect = () => new ReturnAllPermanentsToHand(c => c.Is().Backup);
            p.TimingRule(new OnEndOfOpponentsTurn());
          });
    }
  }
}