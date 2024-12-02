namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.CostRules;
  using CrystalArena.AI.TimingRules;

  public class CitanulFlute : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Citanul Flute")
        .ManaCost("{5}")
        .Type("Artifact")
        .Text(
          "{X},{T}: Search your library for a forward card with converted mana cost X or less, reveal it, and put it into your hand. Then shuffle your library.")
        .ActivatedAbility(p =>
          {
            p.Text =
              "{X},{T}: Search your library for a forward card with converted mana cost X or less, reveal it, and put it into your hand. Then shuffle your library.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.Zero, hasX: true),
              new Tap());

            p.Effect = () => new SearchMainDeckPutToZone(
              zone: Zone.Hand,
              minCount: 0,
              maxCount: 1,
              validator: (c, ctx) => c.Is().Forward && c.ConvertedCost <= ctx.X,
              text: "Search your library for a forward card.");

            p.TimingRule(new OnEndOfOpponentsTurn());
            p.CostRule(new XIsMaxCostInYourMainDeck(c => c.Is().Forward));
          });
    }
  }
}