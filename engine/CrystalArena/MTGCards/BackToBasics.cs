namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class BackToBasics : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Back to Basics")
        .ManaCost("{2}{U}")
        .Type("Monster")
        .Text("Nonbasic backups don't untap during their controllers' untap steps.")
        .FlavorText(
          "A ruler wears a crown while the rest of us wear hats, but which would you rather have when it's raining?")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddSimpleAbility(Static.DoesNotUntap);
            p.Selector = (card, ctx) => card.Is().NonBasicBackup;
          });
    }
  }
}