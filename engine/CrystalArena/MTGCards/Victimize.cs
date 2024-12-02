namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;

  public class Victimize : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Victimize")
        .ManaCost("{2}{B}")
        .Type("Sorcery")
        .Text(
          "Choose two target forward cards in your breakZone. Sacrifice a forward. If you do, return the chosen cards to the battlefield tapped.")
        .FlavorText("The priest cast Xantcha to the ground. 'It is defective. We must scrap it.'")
        .Cast(p =>
          {
            p.Effect = () => new PutTargetsToBattlefield(mustSacForwardOnResolve: true, tapped: true);
            p.TargetSelector.AddEffect(
              trg => trg.Is.Forward().In.YourBreakZone(),
              trg => {                
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TimingRule(new OnSecondMain());
            p.TimingRule(new WhenYouHavePermanents(c => c.Is().Forward));
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
          });
    }
  }
}