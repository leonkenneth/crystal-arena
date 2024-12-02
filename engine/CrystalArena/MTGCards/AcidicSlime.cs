namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class AcidicSlime : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Acidic Slime")
        .ManaCost("{3}{G}{G}")
        .Type("Forward Ooze")
        .Text(
          "{Deathtouch}{EOL}When Acidic Slime enters the battlefield, destroy target artifact, monster, or backup.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[3])
        .Power(2)
        .Toughness(2)
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .SimpleAbilities(Static.Deathtouch)
        .TriggeredAbility(p =>
          {
            p.Text = "When Acidic Slime enters the battlefield, destroy target artifact, monster, or backup.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(card => card.Is().Artifact || card.Is().Monster || card.Is().Backup)
              .On.Battlefield());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score, ControlledBy.Opponent));
          }
        );
    }
  }
}