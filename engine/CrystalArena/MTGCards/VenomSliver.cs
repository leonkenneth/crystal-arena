namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Modifiers;

  public class VenomSliver : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Venom Sliver")
        .ManaCost("{1}{G}")
        .Type("Forward — Sliver")
        .Text("Sliver forwards you control have deathtouch.{I}(Any amount of damage a forward with deathtouch deals to a forward is enough to destroy it.){/I}")
        .FlavorText("\"We attacked with arrows dipped in poison. The slivers that did not die began to change.\"{EOL}—Hastric, Thunian scout")
        .Power(1)
        .Toughness(1)
        .ContinuousEffect(p =>
        {
          p.Selector = (card, ctx) => card.Is("Sliver") && card.Controller == ctx.You;
          p.Modifier = () => new AddSimpleAbility(Static.Deathtouch);
        });
    }
  }
}
