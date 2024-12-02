namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;

  public class DayOfJudgment : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Day of Judgment")
        .ManaCost("{2}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all forwards.")
        .Cast(p =>
          {
            p.TimingRule(new OnSecondMain());
            p.Effect = () => new DestroyAllPermanents((c, ctx) => c.Is().Forward);
          });
    }
  }
}