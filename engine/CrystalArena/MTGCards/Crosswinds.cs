namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class Crosswinds : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Crosswinds")
        .ManaCost("{1}{G}")
        .Type("Monster")
        .Text("Forwards with flying get -2/-0.")
        .FlavorText(
          "Harbin's ornithopter had been trapped for two days within the currents of the storm. When the skies cleared, all he could see was a horizon of trees.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddPowerAndToughness(-2, 0);
            p.Selector = (card, ctx) => card.Is().Forward && card.Has().Flying;
          });
    }
  }
}