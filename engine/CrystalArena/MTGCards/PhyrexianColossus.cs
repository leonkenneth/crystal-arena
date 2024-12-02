namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using Modifiers;

  public class PhyrexianColossus : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phyrexian Colossus")
        .ManaCost("{7}")
        .Type("Artifact Forward Golem")
        .Text(
          "Phyrexian Colossus doesn't untap during your untap step.{EOL}Pay 8 life: Untap Phyrexian Colossus.{EOL}Phyrexian Colossus can't be blocked except by three or more forwards.")
        .Power(8)
        .Toughness(8)
        .StaticAbility(p => p.Modifier(() => new SetMinBlockerCount(3)))
        .SimpleAbilities(Static.DoesNotUntap)
        .ActivatedAbility(p =>
          {
            p.Text = "Pay 8 life: Untap Phyrexian Colossus.";
            p.Cost = new PayLife(8);
            p.Effect = () => new UntapOwner();

            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => c.IsTapped));            
            p.TimingRule(new OnStep(Step.BeginningOfCombat));
          });
    }
  }
}