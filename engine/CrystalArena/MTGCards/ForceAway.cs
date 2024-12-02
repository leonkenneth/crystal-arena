namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class ForceAway : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Force Away")
        .ManaCost("{1}{U}")
        .Type("Summon")
        .Text(
          "Return target forward to its owner's hand.{EOL}{I}Ferocious{/I} — If you control a forward with power 4 or greater, you may draw a card. If you do, discard a card.")
        .FlavorText("Where an enemy once rode, not even a whisper remains.")
        .Cast(p =>
        {
          p.Effect = () => new FerociousEffect(
            L(new ReturnToHand()),
            L(new DrawCards(1, discardCount: 1)));

          p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

          p.TargetingRule(new EffectBounce());
          p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Bounce, EffectTag.ForwardsOnly));
        });
    }
  }
}
