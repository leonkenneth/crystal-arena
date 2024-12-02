namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class FirstResponse : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("First Response")
        .ManaCost("{3}{W}")
        .Type("Monster")
        .Text(
          "At the beginning of each upkeep, if you lost life last turn, put a 1/1 light Soldier forward token onto the battlefield.")
        .FlavorText(
          "\"There's never a good time for a disaster or an attack. That's why we're here.\"{EOL}—Oren, militia captain")
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of each upkeep, if you lost life last turn, put a 1/1 light Soldier forward token onto the battlefield.";

            p.Trigger(new OnStepStart(activeTurn: true, passiveTurn: true, step: Step.Upkeep)
              {
                Condition = ctx => ctx.Turn.PrevTurnEvents.HasLostLife(ctx.You)
              });

            p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Soldier")
                .Power(1)
                .Toughness(1)
                .Type("Token Forward - Soldier")
                .Colors(CardColor.Light));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}