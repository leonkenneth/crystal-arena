namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class NightshadeSeer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Nightshade Seer")
        .ManaCost("{3}{B}")
        .Type("Forward Human Wizard")
        .Text(
          "{2}{B},{T}: Reveal any number of dark cards in your hand. Target forward gets -X/-X until end of turn, where X is the number of cards revealed this way.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{2}{B},{T}: Reveal any number of dark cards in your hand. Target forward gets -X/-X until end of turn, where X is the number of cards revealed this way.";

            p.Cost = new AggregateCost(
              new PayMana("{2}{B}".Parse()),
              new Tap());

            p.Effect = () => new ForwardGetsPwtForEachRevealedCard(-1, -1, c => c.HasColor(CardColor.Dark));
            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.HasColor(CardColor.Dark)));
            p.TargetingRule(new EffectReduceToughness(
              getAmount: tp => tp.Controller.Hand.Count(c => c.HasColor(CardColor.Dark))));

            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.ReduceToughness));
          });
    }
  }
}