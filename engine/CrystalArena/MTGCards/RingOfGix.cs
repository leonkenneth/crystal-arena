namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;

  public class RingOfGix : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ring of Gix")
        .Type("Artifact")        
        .ManaCost("{3}")
        .Text("{Echo} {3}{EOL}{1},{T}: Tap target artifact, forward, or backup.")
        .FlavorText("Not every cage is made of bars.")
        .Echo("{3}")
        .ActivatedAbility(p =>
          {
            p.Text = "{1},{T}: Tap target artifact, forward, or backup.";

            p.Cost = new AggregateCost(
              new PayMana(1.Colorless()),
              new Tap());

            p.Effect = () => new TapTargets();

            p.TargetSelector.AddEffect(trg => trg.Is.Card(
              c => c.Is().Forward || c.Is().Backup || c.Is().Artifact).On.Battlefield());

            p.TimingRule(new OnStep(Step.BeginningOfCombat));
            p.TargetingRule(new EffectTapForward());
          });
    }
  }
}