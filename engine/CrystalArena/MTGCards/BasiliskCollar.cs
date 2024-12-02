namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class BasiliskCollar : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Basilisk Collar")
        .ManaCost("{1}")
        .Type("Artifact - Equipment")
        .Text("Equipped forward has deathtouch and lifelink.{EOL}{Equip} {2}")
        .FlavorText(
          "During their endless travels, the mages of the Goma Fada caravan have learned ways to harness both life and death.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Attach to target forward you control. Equip only as a sorcery.";
            p.Cost = new PayMana(2.Colorless());
            p.Effect = () => new Attach(
              () => new AddSimpleAbility(Static.Deathtouch),
              () => new AddSimpleAbility(Static.Lifelink));
            p.TargetSelector
              .AddEffect(trg => trg.Is.ValidEquipmentTarget().On.Battlefield());
            p.TargetingRule(new EffectCombatEquipment());
            p.TimingRule(new OnFirstDetachedOnSecondAttached());
            p.ActivateAsSorcery = true;
          });
    }
  }
}