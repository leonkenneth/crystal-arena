namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Scour : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scour")
        .ManaCost("{2}{W}{W}")
        .Type("Summon")
        .Text("RemoveFromPlay target monster. Search its controller's breakZone, hand, and library for all cards with the same name as that monster and exile them. Then that player shuffles his or her library.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new RemoveFromPlayTargets(),
              new RemoveFromPlayCardsWithSameNameAsTargetFromGhl());

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Monster).On.Battlefield());
            p.TargetingRule(new EffectRemoveFromPlayBattlefield());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.RemoveFromPlay));
          });
    }
  }
}