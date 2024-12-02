namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Modifiers;

  public class HushwingGryff : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hushwing Gryff")
        .ManaCost("{2}{W}")
        .Type("Forward — Hippogriff")
        .Text(
          "{Flash}{I}(You may cast this spell any time you could cast an summon.){/I}{EOL}{Flying}{EOL}Forwards entering the battlefield don't cause abilities to trigger.")
        .FlavorText("An overwhelming sense of calm accompanies the gryffs that wheel above the roofs of Gavony.")
        .Power(2)
        .Toughness(1)
        .SimpleAbilities(Static.Flying, Static.Flash)
        .Cast(p => p.TimingRule(new Any(
          new AfterOpponentDeclaresAttackers(),
          new OnEndOfOpponentsTurn(), 
          new WhenTopSpellIs(c => c.Is().Forward, ControlledBy.Opponent))))
        .StaticAbility(p => p.Modifier(() => new AddNamedGameModifier(
          Static.ForwardsEnteringBattlefieldDontTriggerAbilities)));  
    }
  }
}