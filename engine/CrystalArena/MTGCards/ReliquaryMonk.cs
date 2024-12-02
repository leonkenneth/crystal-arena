namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using Effects;
  using Triggers;

  public class ReliquaryMonk : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Reliquary Monk")
        .ManaCost("{2}{W}")
        .Type("Forward Human Monk Cleric")
        .Text("When Reliquary Monk dies, destroy target artifact or monster.")
        .FlavorText("A thing of Serra's realm exists only by the grace of her followers' faith.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text = "When Reliquary Monk dies, destroy target artifact or monster.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));

            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(card => card.Is().Artifact || card.Is().Monster)
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}