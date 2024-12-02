namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;

  public class Curfew : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Curfew")
        .ManaCost("{U}")
        .Type("Summon")
        .Text("Each player returns a forward he or she controls to its owner's hand.")
        .FlavorText(". . . But I'm not tired'")
        .Cast(p =>
          {
            p.Effect = () => new EachPlayerReturnsCardsToHand(
              minCount: 1,
              maxCount: 1,
              zone: Zone.Battlefield,
              filter: c => c.Is().Forward,
              aiOrdersByDescendingScore: false,
              text: "Select forward to return to hand"
              );

            p.TimingRule(new NonTargetRemovalTimingRule(1));
          });
    }
  }
}