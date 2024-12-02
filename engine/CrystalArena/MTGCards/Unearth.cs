namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;

  public class Unearth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Unearth")
        .ManaCost("{B}")
        .Type("Sorcery")
        .Text(
          "Return target forward card with converted mana cost 3 or less from your breakZone to the battlefield.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new PutTargetsToBattlefield();
            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Forward && c.ConvertedCost <= 3).In.YourBreakZone());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
          });
    }
  }
}