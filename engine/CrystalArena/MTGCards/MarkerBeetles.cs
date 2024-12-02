namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class MarkerBeetles : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Marker Beetles")
        .ManaCost("{1}{G}{G}")
        .Type("Forward Insect")
        .Text(
          "When Marker Beetles dies, target forward gets +1/+1 until end of turn.{EOL}{2}, Sacrifice Marker Beetles: Draw a card.")
        .FlavorText("In case of emergency, crush bug.")
        .Power(2)
        .Toughness(3)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Marker Beetles dies, target forward gets +1/+1 until end of turn.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.BreakZone));

            p.Effect = () => new ApplyModifiersToTargets(() => new AddPowerAndToughness(1, 1)
              {
                UntilEot = true
              });

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TargetingRule(new EffectCombatMonster());
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}, Sacrifice Marker Beetles: Draw a card.";

            p.Cost = new AggregateCost(
              new PayMana(2.Colorless()),
              new Sacrifice());

            p.Effect = () => new DrawCards(1);

            p.TimingRule(new Any(
              new WhenOwningCardWillBeDestroyed(),
              new OnStep(Step.DeclareBlocker)));

            p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
          });
    }
  }
}