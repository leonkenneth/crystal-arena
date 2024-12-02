namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;

  public class QuicksilverAmulet : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Quicksilver Amulet")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text("{4},{T}: You may put a forward card from your hand onto the battlefield.")
        .FlavorText("Wonderful You got a lion on your first try. Now put it back.")
        .ActivatedAbility(p =>
          {
            p.Text = "{4},{T}: You may put a forward card from your hand onto the battlefield.";

            p.Cost = new AggregateCost(
              new PayMana(4.Colorless()),
              new Tap());

            p.Effect = () => new PutSelectedCardsToBattlefield(
              text: "Select a forward in your hand.",
              fromZone: Zone.Hand,
              validator: card => card.Is().Forward);

            p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new OnEndOfOpponentsTurn()));
            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.Is().Forward));
          });
    }
  }
}