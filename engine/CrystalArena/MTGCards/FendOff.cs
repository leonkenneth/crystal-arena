namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class FendOff : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fend Off")
        .ManaCost("{1}{W}")
        .Type("Summon")
        .Text(
          "Prevent all combat damage that would be dealt by target forward this turn.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("The best defense is to not get hit.")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new PreventAllDamageFromSourceUntilEot(preventCombatOnly: true);
            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TargetingRule(new EffectPreventCombatDamage());
            p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new AfterOpponentDeclaresBlockers()));
          });
    }
  }
}