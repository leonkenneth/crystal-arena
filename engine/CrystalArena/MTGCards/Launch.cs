namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;
 
  public class Launch : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Launch")
        .ManaCost("{1}{U}")
        .Type("Monster - Aura")
        .Text(
          "Enchanted forward has flying.{EOL}When Launch is put into a breakZone from the battlefield, return Launch to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new AddSimpleAbility(Static.Flying));
            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatMonster(filter: x => !x.Has().Flying));
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Launch is put into a breakZone from the battlefield, return Launch to its owner's hand.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}