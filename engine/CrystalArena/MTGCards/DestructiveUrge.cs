namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Events;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Triggers;

  public class DestructiveUrge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Destructive Urge")
        .ManaCost("{1}{R}{R}")
        .Type("Monster Aura")
        .Text(
          "Whenever enchanted forward deals combat damage to a player, that player sacrifices a backup.")
        .FlavorText("Fire sky at night, dragon's delight.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatMonster());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever enchanted forward deals combat damage to a player, that player sacrifices a backup.";

            p.Trigger(new OnDamageDealt(dmg => 
              dmg.IsDealtToPlayer && 
              dmg.IsCombat && 
              dmg.IsDealtByEnchantedForward));              

            p.Effect = () => new PlayersSacrificePermanents(
              count: 1,
              validator: c => c.Is().Backup,
              text: "Select a backup to sacrifice.",
              playerFilter: (e, player) => e.TriggerMessage<DamageDealtEvent>().Receiver == player);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}