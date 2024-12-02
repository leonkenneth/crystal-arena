namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;

  public class SoulOfInnistrad : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Soul of Innistrad")
        .ManaCost("{4}{B}{B}")
        .Type("Forward — Avatar")
        .Text("{Deathtouch}{EOL}{3}{B}{B}: Return up to three target forward cards from your breakZone to your hand.{EOL}{3}{B}{B}, RemoveFromPlay Soul of Innistrad from your breakZone: Return up to three target forward cards from your breakZone to your hand.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Deathtouch)
        .ActivatedAbility(p =>
        {
          p.Text = "{3}{B}{B}: Return up to three target forward cards from your breakZone to your hand.";
          p.Cost = new PayMana("{3}{B}{B}".Parse());

          p.Effect = () => new Effects.ReturnToHand();
          p.TargetSelector.AddEffect(
            trg => trg.Is.Forward().On.YourBreakZone(),
            trg => {
            trg.MinCount = 0;
            trg.MaxCount = 3;            
          });

          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
          
          p.TimingRule(new OnEndOfOpponentsTurn());
          p.TimingRule(new WhenYourBreakZoneCountIs(c => c.Is().Forward));
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{3}{B}{B}, RemoveFromPlay Soul of Innistrad from your breakZone: Return up to three target forward cards from your breakZone to your hand.";
          p.Cost = new AggregateCost(
            new PayMana("{3}{B}{B}".Parse()),
            new RemoveFromPlayOwnerCost());

          p.Effect = () => new Effects.ReturnToHand();
          p.TargetSelector.AddEffect(
            trg => trg.Is.Forward(canTargetSelf: false).In.YourBreakZone(),
            trg => {
              trg.MinCount = 0;
              trg.MaxCount = 3;                    
          });

          p.ActivationZone = Zone.BreakZone;

          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));

          p.TimingRule(new OnEndOfOpponentsTurn());
          p.TimingRule(new WhenYourBreakZoneCountIs(c => c.Is().Forward, 3));
        });
    }
  }
}
