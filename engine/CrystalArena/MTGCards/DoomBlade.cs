namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;

  public class DoomBlade : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Doom Blade")
        .ManaCost("{1}{B}")
        .Type("Summon")
        .Text("Destroy target nonblack forward.")
        .FlavorText("The void is without substance but cuts like steel.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Forward && !c.HasColor(CardColor.Dark))
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule()
              .RemovalTags(EffectTag.Destroy, EffectTag.ForwardsOnly));
          });
    }
  }
}