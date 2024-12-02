namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
    using CrystalArena.AI.TimingRules;

    public class GrimContest : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Grim Contest")
        .ManaCost("{1}{B}{G}")
        .Type("Summon")
        .Text(
          "Choose target forward you control and target forward an opponent controls. Each of those forwards deals damage equal to its toughness to the other.")
        .FlavorText("The invader hoped he could survive the beast's jaws and emerge through its rotting skin.")
        .Cast(p =>
          {
            p.Effect = () => new Fight(c => c.Toughness ?? 0);

            p.TargetSelector.AddEffect(
              trg => trg.Is.Forward(ControlledBy.SpellOwner).On.Battlefield(),
              trg => { trg.Message = "Select a target forward you control."; });

            p.TargetSelector.AddEffect(
              trg => trg.Is.Forward(ControlledBy.Opponent).On.Battlefield(),
              trg => { trg.Message = "Select a target forward your oppenent controls."; });

            p.TargetingRule(new EffectFight(c => c.Toughness ?? 0));
            p.TimingRule(new Any(new OnMainStepsOfYourTurn(), new AfterOpponentDeclaresAttackers()));
          });
    }
  }
}