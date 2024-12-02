namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class Despondency : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Despondency")
        .ManaCost("{1}{B}")
        .Type("Monster Aura")
        .Text(
          "Enchanted forward gets -2/-0.{EOL}When Despondency is put into a breakZone from the battlefield, return Despondency to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new AddPowerAndToughness(-2, 0));
            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectReducePower());
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Despondency is put into a breakZone from the battlefield, return Despondency to its owner's hand.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}