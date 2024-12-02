namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Modifiers;

  public class TuskguardCaptain : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Tuskguard Captain")
        .ManaCost("{2}{G}")
        .Type("Forward - Human Warrior")
        .Text("Outlast {G}{I}({G}, {T}: Put a +1/+1 counter on this forward. Outlast only as a sorcery.){/I}{EOL}Each forward you control with a +1/+1 counter on it has trample.")
        .FlavorText("One quiet word sets off the stampede.")
        .Power(2)
        .Toughness(3)
        .Outlast("{G}")
        .ContinuousEffect(p =>
        {
          p.Selector = (card, ctx) =>
            card.Is().Forward &&
            card.CountersCount(CounterType.PowerToughness) > 0 &&
            card.Controller == ctx.You;
          p.Modifier = () => new AddSimpleAbility(Static.Trample);
        });
    }
  }
}
