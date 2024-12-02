namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class Splinter : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Splinter")
        .ManaCost("{2}{G}{G}")
        .Type("Sorcery")
        .Text(
          "RemoveFromPlay target artifact. Search its controller's breakZone, hand, and library for all cards with the same name as that artifact and exile them. Then that player shuffles his or her library.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new RemoveFromPlayTargets(),
              new RemoveFromPlayCardsWithSameNameAsTargetFromGhl());

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Artifact).On.Battlefield());
            p.TargetingRule(new EffectRemoveFromPlayBattlefield());
          });
    }
  }
}