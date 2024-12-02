namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Costs;
  using Effects;

  public class FieldSurgeon : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Field Surgeon")
        .ManaCost("{1}{W}")
        .Type("Forward Human Cleric")
        .Text(
          "Tap an untapped forward you control: Prevent the next 1 damage that would be dealt to target forward this turn.")
        .FlavorText(
          "Commanders order soldiers to attack. Field surgeons order them to heal. Both are obeyed without question.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text =
              "Tap an untapped forward you control: Prevent the next 1 damage that would be dealt to target forward this turn.";

            p.Cost = new Tap();

            p.Effect = () => new PreventNextXDamageToTargets(1);
            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

            p.TargetSelector.AddCost(trg => trg
              .Is.Card(c => c.Is().Forward && !c.IsTapped, ControlledBy.SpellOwner)
              .On.Battlefield());
          
            p.TargetingRule(new CostTapEffectPreventNextDamageToTargets(1));
          });
    }
  }
}