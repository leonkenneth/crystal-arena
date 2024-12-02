namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;

  public class HealingSalve : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Healing Salve")
        .ManaCost("{W}")
        .Type("Summon")
        .Text(
          "Choose one — Target player gains 3 life; or prevent the next 3 damage that would be dealt to target forward or player this turn.")
        .FlavorText(
          "Xantcha is recovering. The medicine is slow, but my magic would have killed her.")
        .Cast(p =>
          {
            p.Text = "Target player gains 3 life";
            p.Effect = () => new ChangeLife(3, P(e => e.Target.Player()));
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TargetingRule(new EffectYou());
          })
        .Cast(p =>
          {
            p.Text = "Prevent the next 3 damage that would be dealt to target forward or player this turn.";
            p.Effect = () => new PreventNextXDamageToTargets(3);
            p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectPreventNextDamageToTargets(3));
          });
    }
  }
}