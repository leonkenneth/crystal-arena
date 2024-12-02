namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class RofellossGift : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rofellos's Gift")
        .ManaCost("{G}")
        .Type("Sorcery")
        .Text(
          "Reveal any number of wind cards in your hand. Return an monster card from your breakZone to your hand for each card revealed this way.")
        .FlavorText("Rise, elf. We are both of Gaea, and thus we are equal.")
        .Cast(p =>
          {
            p.Effect = () => new ReturnCardsFromBreakZoneToHandForEachRevealedCard(
              revealFilter: c => c.HasColor(CardColor.Wind),
              breakZoneFilter: c => c.Is().Monster);

            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.HasColor(CardColor.Wind)));
            p.TimingRule(new WhenYourBreakZoneCountIs(minCount: 1, selector: c => c.Is().Monster));
          });
    }
  }
}