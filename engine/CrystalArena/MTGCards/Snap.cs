namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;

  public class Snap : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Snap")
        .ManaCost("{1}{U}")
        .Type("Summon")
        .Text("Return target forward to its owner's hand. Untap up to two backups.")
        .FlavorText("Good riddance.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new ReturnToHand(),
              new UntapSelectedPermanents(
                minCount: 0,
                maxCount: 2,
                validator: c => c.Is().Backup,
                text: "Select backups to untap."
                )
              );

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

            p.TargetingRule(new EffectBounce());
            p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Bounce, EffectTag.ForwardsOnly));
          });
    }
  }
}