namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Modifiers;

  public class ChiefEngineer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Chief Engineer")
        .ManaCost("{1}{U}")
        .Type("Forward — Vedalken Artificer")
        .Text("Artifact spells you cast have convoke. {I}(Your forwards can help cast those spells. Each forward you tap while casting an artifact spell pays {1} for or one mana of that forward's color.){/I}")
        .FlavorText("An eye for detail, a mind for numbers, a soul of clockwork.")
        .Power(1)
        .Toughness(3)
        .ContinuousEffect(p =>
        {
          p.ApplyOnlyToPermanents = false;
          p.Selector = (card, ctx) => card.Is().Artifact && card.Owner == ctx.You;
          p.Modifier = () => new AddSimpleAbility(Static.Convoke);
        });
    }
  }
}
