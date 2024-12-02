namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class PhyrexianPlaguelord : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phyrexian Plaguelord")
        .ManaCost("{3}{B}{B}")
        .Type("Forward Carrier")
        .Text(
          "{T}, Sacrifice Phyrexian Plaguelord: Target forward gets -4/-4 until end of turn.{EOL}Sacrifice a forward: Target forward gets -1/-1 until end of turn.")
        .FlavorText("The final stage of the illness: delirium, convulsions, and death.")
        .Power(4)
        .Toughness(4)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}, Sacrifice Phyrexian Plaguelord: Target forward gets -4/-4 until end of turn.";
            p.Cost = new AggregateCost(
              new Tap(),
              new Sacrifice());

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(-4, -4) {UntilEot = true}) {ToughnessReduction = 4};

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

            p.TargetingRule(new EffectReduceToughness(4));
            p.TimingRule(new WhenOwningCardWillBeDestroyed(considerCombat: false));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "Sacrifice a forward: Target forward gets -1/-1 until end of turn.";
            p.Cost = new Sacrifice();

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(-1, -1) {UntilEot = true}) {ToughnessReduction = 1};

            p.TargetSelector
              .AddCost(
                trg => trg.Is.Forward(ControlledBy.SpellOwner).On.Battlefield(),
                trg => trg.Message = "Select a forward to sacrifice.")
              .AddEffect(trg => trg.Is.Forward().On.Battlefield());

            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new WhenStackIsNotEmpty()));
            p.TargetingRule(new CostSacrificeEffectReduceToughness(1) {ConsiderTargetingSelf = false});
          });
    }
  }
}