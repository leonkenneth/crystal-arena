namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class KarmicGuide : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Karmic Guide")
        .ManaCost("{3}{W}{W}")
        .Type("Forward Angel Spirit")
        .Text(
          "{Flying}, {protection from dark}{EOL}When Karmic Guide enters the battlefield, return target forward card from your breakZone to the battlefield.{EOL}{Echo} {3}{W}{W}")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[3])
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Flying)
        .Protections(CardColor.Dark)
        .Echo("{3}{W}{W}")
        .Cast(p => p.TimingRule(new WhenYourBreakZoneCountIs(c => c.Is().Forward, minCount: 1)))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Karmic Guide enters the battlefield, return target forward card from your breakZone to the battlefield.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new PutTargetsToBattlefield();
            p.TargetSelector.AddEffect(trg => trg.Is.Forward().In.YourBreakZone());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
          });
    }
  }
}