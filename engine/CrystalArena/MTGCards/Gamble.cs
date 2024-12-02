namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;

  public class Gamble : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Gamble")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text(
          "Search your library for a card, put that card into your hand, discard a card at random, then shuffle your library.")
        .FlavorText("When you've got nothing, you might as well trade it for something else.")
        .Cast(p =>
          {
            p.Effect =
              () => new SearchMainDeckPutToZone(
                zone: Zone.Hand,
                minCount: 1,
                maxCount: 1,
                revealCards: false)
                {AfterResolve = ctx => ctx.You.DiscardRandomCard()};

            p.TimingRule(new OnFirstMain());
          });
    }
  }
}