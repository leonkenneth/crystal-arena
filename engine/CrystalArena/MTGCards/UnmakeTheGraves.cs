namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class UnmakeTheGraves : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Unmake the Graves")
        .ManaCost("{4}{B}")
        .Type("Summon")
        .Text("{Convoke} {I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}Return up to two target forward cards from your breakZone to your hand.")
        .FlavorText("\"I'm raising an army. Any volunteers?\"")
        .SimpleAbilities(Static.Convoke)
        .Cast(p =>
        {
          p.Text = "Return up to two target forward cards from your breakZone to your hand.";
          p.Effect = () => new ReturnToHand();
          p.TargetSelector.AddEffect(
            trg => trg.Is.Forward().On.YourBreakZone(),
            trg => {
              trg.MinCount = 0;
              trg.MaxCount = 2;            
            });

          p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));

          p.TimingRule(new OnEndOfOpponentsTurn());
          p.TimingRule(new WhenYourBreakZoneCountIs(c => c.Is().Forward, 2));
        });
    }
  }
}
