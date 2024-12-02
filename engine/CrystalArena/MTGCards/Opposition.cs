namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class Opposition : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opposition")
        .ManaCost("{2}{U}{U}")
        .Type("Monster")
        .Text("Tap an untapped forward you control: Tap target artifact, forward, or backup.")
        .FlavorText("Urza says he's sane. Perhaps, but measures of sanity among planeswalkers are hard to come by.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "Tap an untapped forward you control: Tap target artifact, forward, or backup.";
            p.Cost = new Tap();
            p.Effect = () => new TapTargets();

            p.TargetSelector
              .AddCost(
                trg => trg.Is.Forward(ControlledBy.SpellOwner).On.Battlefield(),
                trg => { trg.Message = "Select a forward you control."; })
              .AddEffect(
                trg => trg.Is.Card(c => c.Is().Forward || c.Is().Artifact || c.Is().Backup).On.Battlefield(),
                trg => { trg.Message = "Select an artifact, forward or backup."; });

            p.TimingRule(new OnOpponentsTurn(Step.Upkeep));
            p.TargetingRule(new CostTapEffectTap());
          });
    }
  }
}