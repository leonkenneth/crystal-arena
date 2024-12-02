namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class GoblinGardener : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Gardener")
        .ManaCost("{3}{R}")
        .Type("Forward Goblin")
        .Text("When Goblin Gardener dies, destroy target backup.")
        .FlavorText("Grow food in dirt? Save time—eat dirt.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
        .Power(2)
        .Toughness(1)
        .TriggeredAbility(p =>
          {
            p.Text = "When Goblin Gardener dies, destroy target backup.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Backup).On.Battlefield());
            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}