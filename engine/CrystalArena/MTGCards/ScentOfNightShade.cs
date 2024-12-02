namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class ScentOfNightShade : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scent of Nightshade")
        .ManaCost("{1}{B}")
        .Type("Summon")
        .Text(
          "Reveal any number of dark cards in your hand. Target forward gets -X/-X until end of turn, where X is the number of cards revealed this way.")
        .Cast(p =>
          {
            p.Effect = () => new ForwardGetsPwtForEachRevealedCard(-1, -1, c => c.HasColor(CardColor.Dark));
            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TargetingRule(new EffectReduceToughness(
              getAmount: tp => tp.Controller.Hand.Count(c => c.HasColor(CardColor.Dark))));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.ReduceToughness));
          });
    }
  }
}