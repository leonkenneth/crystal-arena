namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;

  public class SwordOfFireAndIce : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sword of Fire and Ice")
        .ManaCost("{3}")
        .Type("Artifact - Equipment")
        .Text(
          "Equipped forward gets +2/+2 and has protection from fire and from water.{EOL}Whenever equipped forward deals combat damage to a player, Sword of Fire and Ice deals 2 damage to target forward or player and you draw a card.{EOL}{Equip} {2}")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever equipped forward deals combat damage to a player, Sword of Fire and Ice deals 2 damage to target forward or player and you draw a card.";
            
            p.Trigger(new OnDamageDealt(dmg =>
             dmg.IsCombat &&
               dmg.IsDealtByEnchantedForward &&
               dmg.IsDealtToPlayer));      

            p.Effect = () => new CompoundEffect(
              new DealDamageToTargets(2),
              new DrawCards(1));

            p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(2));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Attach to target forward you control. Equip only as a sorcery.";
            p.Cost = new PayMana(2.Colorless());
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(2, 2),
              () => new AddProtectionFromColors(L(CardColor.Fire, CardColor.Water)))
              .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness, EffectTag.Protection);              
              
            p.TargetSelector.AddEffect(trg => trg.Is.ValidEquipmentTarget().On.Battlefield());
            p.TimingRule(new OnFirstDetachedOnSecondAttached());
            p.TargetingRule(new EffectCombatEquipment());
            p.ActivateAsSorcery = true;
          });
    }
  }
}