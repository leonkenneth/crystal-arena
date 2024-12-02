namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class SneakAttack : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sneak Attack")
        .ManaCost("{3}{R}")
        .Type("Monster")
        .Text(
          "{R}: You may put a forward card from your hand onto the battlefield. That forward gains haste. Sacrifice the forward at the beginning of the next end step.")
        .FlavorText("Nothin' beat surprise—'cept rock.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text =
              "{R}: You may put a forward card from your hand onto the battlefield. That forward gains haste. Sacrifice the forward at the beginning of the next end step.";
            p.Cost = new PayMana(Mana.Fire);

            p.Effect = () => new PutSelectedCardsToBattlefield(
              text: "Select a forward card in your hand.",
              validator: c => c.Is().Forward,
              fromZone: Zone.Hand,
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
            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.Is().Forward));
          });
    }
  }
}