namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;

  public class SwordsToPlowshares : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Swords to Plowshares")
        .ManaCost("{W}")
        .Type("Summon")
        .Text("RemoveFromPlay target forward. Its controller gains life equal to its power.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new RemoveFromPlayTargets(),
              new ChangeLife(
                amount: P(e => e.Target.Card().Power.GetValueOrDefault()),
                whos: P(e => e.Target.Card().Controller)));

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            
            p.TargetingRule(new EffectRemoveFromPlayBattlefield());
            p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.RemoveFromPlay, EffectTag.ForwardsOnly));
          });
    }
  }
}