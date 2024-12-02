namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class ShieldOfTheAvatar : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Shield of the Avatar")
        .ManaCost("{1}")
        .Type("Artifact — Equipment")
        .Text("If a source would deal damage to equipped forward, prevent X of that damage, where X is the number of forwards you control.{EOL}Equip {2}({2}: Attach to target forward you control. Equip only as a sorcery.)")
        .FlavorText("We are made stronger by those we fight for.")
        .ActivatedAbility(p =>
        {
          p.Text = "Equip {2} ({2}: Attach to target forward you control. Equip only as a sorcery.)";

          p.Cost = new PayMana(2.Colorless());

          p.Effect = () => new CompoundEffect(
            new Attach(),
            new PreventDamageToEquipedForward(amount: forward => 
              forward.Controller.Battlefield.Forwards.Count()));
           
          p.TargetSelector.AddEffect(trg => trg.Is.ValidEquipmentTarget().On.Battlefield());

          p.TargetingRule(new EffectCombatEquipment());
          p.TimingRule(new OnFirstDetachedOnSecondAttached());

          p.ActivateAsSorcery = true;
        });
    }
  }
}
