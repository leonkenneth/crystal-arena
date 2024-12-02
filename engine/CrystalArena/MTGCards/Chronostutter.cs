namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Chronostutter : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Chronostutter")
        .ManaCost("{5}{U}")
        .Type("Summon")
        .Text("Put target forward into its owner's library second from the top.")
        .FlavorText("Timing is everything.")
        .Cast(p =>
        {
          p.Text = "Put target forward into its owner's library second from the top.";

          p.Effect = () => new PutTargetsIntoMainDeckAtPosition(positionFromTheTop: 1);

          p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

          p.TargetingRule(new EffectPutOnTopOfMainDeck());
          p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Bounce, EffectTag.ForwardsOnly));
        });
    }
  }
}
