namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;  

  public class Eradicate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Eradicate")
        .ManaCost("{2}{B}{B}")
        .Type("Sorcery")
        .Text(
          "RemoveFromPlay target nonblack forward. Search its controller's breakZone, hand, and library for all cards with the same name as that forward and exile them. Then that player shuffles his or her library.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new RemoveFromPlayTargets(),
              new RemoveFromPlayCardsWithSameNameAsTargetFromGhl());

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Forward && !c.HasColor(CardColor.Dark)).On.Battlefield());
            p.TargetingRule(new EffectRemoveFromPlayBattlefield());
          });
    }
  }
}