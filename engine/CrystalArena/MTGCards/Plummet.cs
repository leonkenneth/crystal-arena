namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Plummet : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Plummet")
        .ManaCost("{1}{G}")
        .Type("Summon")
        .Text("Destroy target forward with flying.")
        .FlavorText("\"Let nothing own the skies but the wind.\"{EOL}—Dejara, Giltwood druid")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Forward && c.Has().Flying).On.Battlefield());

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Destroy, EffectTag.ForwardsOnly));
          });
    }
  }
}