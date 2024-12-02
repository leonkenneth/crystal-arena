namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Modifiers;

  public class Opalescence : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opalescence")
        .ManaCost("{2}{W}{W}")
        .Type("Monster")
        .Text(
          "Each other non-Aura monster is a forward with power and toughness each equal to its converted mana cost. It's still an monster.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenYouHavePermanents(c => c.Is().Monster && !c.Is().Aura));
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new ChangeToForward(
              power: m => m.OwningCard.ConvertedCost,
              toughness: m => m.OwningCard.ConvertedCost,
              type: m => m.OwningCard.Type.Add(baseTypes: "forward"));

            p.Selector = (card, ctx) => card != ctx.Source && card.Is().Monster && !card.Is().Aura;
          });
    }
  }
}