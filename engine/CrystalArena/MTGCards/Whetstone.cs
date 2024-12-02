namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;

  public class Whetstone : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Whetstone")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{3}: Each player puts the top two cards of his or her library into his or her breakZone.")
        .FlavorText("To hone swords and dull minds.")
        .ActivatedAbility(p =>
          {
            p.Text = "{3}: Each player puts the top two cards of his or her library into his or her breakZone.";
            p.Cost = new PayMana(3.Colorless());
            p.Effect = () => new EachPlayerPutTopCardsFromMainDeckToBreakZone(2);
            p.TimingRule(new OnOpponentsTurn(Step.Upkeep));
          });
    }
  }
}