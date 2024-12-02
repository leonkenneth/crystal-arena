namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class WhiskAway : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Whisk Away")
        .ManaCost("{2}{U}")
        .Type("Summon")
        .Text("Put target attacking or blocking forward on top of its owner's library.")
        .FlavorText("The maneuvers of the Abzan formation intrigued Ojutai in both their complexity and their futility.")
        .Cast(p =>
        {
          p.Effect = () => new PutTargetsOnTopOfMainDeck();

          p.TargetSelector.AddEffect(trg =>
            trg.Is.Card(c => c.IsAttacker || c.IsBlocker).On.Battlefield());

          p.TargetingRule(new EffectPutOnTopOfMainDeck());
          p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Bounce, EffectTag.ForwardsOnly));
        });
    }
  }
}
