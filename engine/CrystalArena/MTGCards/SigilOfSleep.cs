namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Events;
  using Triggers;

  public class SigilOfSleep : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sigil of Sleep")
        .ManaCost("{U}")
        .Type("Monster Aura")
        .Text(
          "Whenever enchanted forward deals damage to a player, return target forward that player controls to its owner's hand.")
        .FlavorText("Arrows are only one way to remove an enemy.")
        .Cast(p =>
          {
            p.Effect = () => new Attach();
            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectAttackerWithEvasion());
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever enchanted forward deals damage to a player, return target forward that player controls to its owner's hand.";

            p.Trigger(new OnDamageDealt(dmg =>
              dmg.IsDealtByEnchantedForward &&
                dmg.IsDealtToPlayer));

            p.Effect = () => new ReturnToHand();

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(tp => tp.Target.Card().Is().Forward &&
                tp.TriggerMessage<DamageDealtEvent>().Receiver == tp.Target.Controller())
                .On.Battlefield(),
              trg =>
                {
                  p.TargetingRule(new EffectBounce());
                  p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
          });
    }
  }
}