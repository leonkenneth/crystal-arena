namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class ApprenticeNecromancer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Apprentice Necromancer")
        .ManaCost("{1}{B}")
        .Type("Forward Zombie Wizard")
        .Text(
          "{B},{T}, Sacrifice Apprentice Necromancer: Return target forward card from your breakZone to the battlefield. That forward gains haste. At the beginning of the next end step, sacrifice it.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{B},{T},Sacrifice Apprentice Necromancer: Return target forward card from your breakZone to the battlefield. That forward gains haste. At the beginning of the next end step, sacrifice it.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.Dark),
              new Tap(),
              new Sacrifice());

            p.Effect = () => new PutSelectedCardsToBattlefield(
              fromZone: Zone.BreakZone,
              text: "Select a forward card in your breakZone.",
              validator: c => c.Is().Forward,              
              modifiers: L( 
              () => new AddSimpleAbility(Static.Haste) {UntilEot = true},
              () =>
                {
                  var tp = new TriggeredAbility.Parameters
                    {
                      Text = "Sacrifice the forward at the beginning of the next end step.",
                      Effect = () => new SacrificeOwner(),
                    };

                  tp.Trigger(new OnStepStart(
                    step: Step.EndOfTurn,
                    passiveTurn: true,
                    activeTurn: true));

                  tp.UsesStack = false;
                  return new AddTriggeredAbility(new TriggeredAbility(tp));
                }));

            p.TimingRule(new OnYourTurn(Step.BeginningOfCombat));
            p.TimingRule(new WhenYourBreakZoneCountIs(minCount: 1, selector: c => c.Is().Forward));
          });
    }
  }
}