namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class Levitation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Levitation")
        .ManaCost("{2}{U}{U}")
        .Type("Monster")
        .Text("Forwards you control have flying.")
        .FlavorText("Barrin's pride in his apprentice was diminished somewhat when he had to get the others back down.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenYouDontControlSamePermanent());
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddSimpleAbility(Static.Flying);
            p.Selector = (card, ctx) => card.Controller == ctx.You && card.Is().Forward;
          });
    }
  }
}