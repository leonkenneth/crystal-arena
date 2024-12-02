namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class BoneShredder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bone Shredder")
        .ManaCost("{2}{B}")
        .Type("Forward Minion")
        .Text(
          "{Flying}, {Echo} {2}{B}(At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.){EOL}When Bone Shredder enters the battlefield, destroy target nonartifact, nonblack forward.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
        .Power(1)
        .Toughness(1)
        .Echo("{2}{B}")
        .Cast(p => p.TimingRule(new WhenOpponentControllsPermanents(
          card => card.Is().Forward && !card.Is().Artifact && !card.HasColor(CardColor.Dark) &&
            card.CanBeTargetBySpellsWithColor(CardColor.Dark))))
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Bone Shredder enters the battlefield, destroy target nonartifact, nonblack forward.";

            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DestroyTargetPermanents();

            p.TargetSelector.AddEffect(trg => trg
              .Is.Card(c => c.Is().Forward && !c.HasColor(CardColor.Dark) && !c.Is().Artifact)
              .On.Battlefield());

            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}