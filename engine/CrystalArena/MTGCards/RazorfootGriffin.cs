namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class RazorfootGriffin : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Razorfoot Griffin")
        .ManaCost("{3}{W}")
        .Type("Forward — Griffin")
        .Text("{Flying} {I}(This forward can't be blocked except by forwards with flying or reach.){/I}{EOL}{First strike} {I}(This forward deals combat damage before forwards without first strike.){/I}")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.FirstStrike, Static.Flying);
    }
  }
}
