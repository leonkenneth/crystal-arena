namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class PathToRemovedFromPlay : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Path to RemoveFromPlay")
        .ManaCost("{W}")
        .Type("Summon")
        .Text("RemoveFromPlay target forward. Its controller may search his or her library for a basic backup card, put that card onto the battlefield tapped, then shuffle his or her library.")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new RemoveFromPlayTargets(),
            new SearchMainDeckPutToZone(
              zone: Zone.Battlefield,
              afterPutToZone: (c, g) => c.Tap(),
              minCount: 0,
              maxCount: 1,
              validator: (c, ctx) => c.Is().BasicBackup,
              text: "Search your library for a basic backup card.",
              player: P(e => e.Target.Controller())));

          p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
          p.TargetingRule(new EffectRemoveFromPlayBattlefield());
        });
    }
  }
}
