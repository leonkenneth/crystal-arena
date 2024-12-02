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

  public class SwordOfBodyAndMind : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sword of Body and Mind")
        .ManaCost("{3}")
        .Type("Artifact - Equipment")
        .Text(
          "Equipped forward gets +2/+2 and has protection from wind and from water.{EOL}Whenever equipped forward deals combat damage to a player, you put a 2/2 wind Wolf forward token onto the battlefield and that player puts the top ten cards of his or her library into his or her breakZone.{EOL}{Equip} {2}")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever equipped forward deals combat damage to a player, you put a 2/2 wind Wolf forward token onto the battlefield and that player puts the top ten cards of his or her library into his or her breakZone.";

            p.Trigger(new OnDamageDealt(dmg =>
              dmg.IsCombat &&
                dmg.IsDealtByEnchantedForward &&
                dmg.IsDealtToPlayer));              

            p.Effect = () => new CompoundEffect(
              new PlayerPutsTopCardsFromMainDeckToBreakZone(P(e => e.Controller.Opponent), count: 10),
              new CreateTokens(
                count: 1,
                token: Card
                  .Named("Wolf")
                  .FlavorText(
                    "No matter where we cat warriors go in the world, those stupid slobberers find us.")
                  .Power(2)
                  .Toughness(2)
                  .Type("Token Forward - Wolf")
                  .Colors(CardColor.Wind)));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Attach to target forward you control. Equip only as a sorcery.";
            p.Cost = new PayMana(2.Colorless());
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(2, 2),
              () => new AddProtectionFromColors(L(CardColor.Wind, CardColor.Water)))
              .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness, EffectTag.Protection);              

            p.TargetSelector.AddEffect(trg => trg.Is.ValidEquipmentTarget().On.Battlefield());
            p.ActivateAsSorcery = true;
            p.TimingRule(new OnFirstDetachedOnSecondAttached());
            p.TargetingRule(new EffectCombatEquipment());
          });
    }
  }
}