namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Triggers;

  public class PhyrexianArena : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phyrexian Arena")
        .ManaCost("{1}{B}{B}")
        .Type("Monster")
        .Text("At the beginning of your upkeep, you draw a card and you lose 1 life.")
        .FlavorText("An audience of one with the malice of thousands.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you draw a card and you lose 1 life.";
            p.Trigger(new OnStepStart(step: Step.Upkeep));
            p.Effect = () => new DrawCards(count: 1, lifeloss: 1);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}