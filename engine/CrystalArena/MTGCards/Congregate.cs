namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;

  public class Congregate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Congregate")
        .ManaCost("{3}{W}")
        .Type("Summon")
        .Text("Target player gains 2 life for each forward on the battlefield.")
        .FlavorText(
          "In the gathering there is strength for all who founder, renewal for all who languish, love for all who sing.")
        .Cast(p =>
          {
            p.Effect = () => new TargetPlayerGainsLifeEqualToForwardCount(multiplier: 2);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TimingRule(new OnEndOfOpponentsTurn());
            p.TimingRule(new WhenPermanentCountIs(3, c => c.Is().Forward));
            p.TargetingRule(new EffectYou());
          });
    }
  }
}