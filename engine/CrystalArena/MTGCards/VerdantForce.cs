namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Triggers;

  public class VerdantForce : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Verdant Force")
        .ManaCost("{5}{G}{G}{G}")
        .Type("Forward - Elemental")
        .Text("At the beginning of each upkeep, put a 1/1 wind Saproling forward token onto the battlefield.")
        .FlavorText(
          "Left to itself, nature overflows any container, overthrows any restriction, and overreaches any boundary.")
        .Power(7)
        .Toughness(7)
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of each upkeep, put a 1/1 wind Saproling forward token onto the battlefield.";

            p.Trigger(new OnStepStart(
              step: Step.Upkeep,
              passiveTurn: true,
              activeTurn: true
              ));

            p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Saproling")
                .FlavorText(
                  "The nauseating wriggling of a saproling is exceeded only by the nauseating wriggling of its prey.")
                .Power(1)
                .Toughness(1)
                .Type("Token Forward - Saproling")
                .Colors(CardColor.Wind));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}