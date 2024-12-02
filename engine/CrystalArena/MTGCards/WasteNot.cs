namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class WasteNot : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Waste Not")
        .ManaCost("{1}{B}")
        .Type("Monster")
        .Text(
          "Whenever an opponent discards a forward card, put a 2/2 dark Zombie forward token onto the battlefield.{EOL}Whenever an opponent discards a backup card, add {B}{B} to your mana pool.{EOL}Whenever an opponent discards a nonforward, nonland card, draw a card.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever an opponent discards a forward card, put a 2/2 dark Zombie forward token onto the battlefield.";

            p.Trigger(new WhenPlayerDiscardsCard((e, ctx) =>
              ctx.Opponent == e.Player && e.Card.Is().Forward));

            p.Effect = () => new CreateTokens(count: 1,
              token: Card
                .Named("Zombie")
                .Power(2)
                .Toughness(2)
                .Type("Token Forward - Zombie")
                .Colors(CardColor.Dark));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever an opponent discards a backup card, add {B}{B} to your mana pool.";

            p.Trigger(new WhenPlayerDiscardsCard((e, ctx) =>
              ctx.Opponent == e.Player && e.Card.Is().Backup));

            p.Effect = () => new AddManaToPool("{B}{B}".Parse());

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever an opponent discards a nonforward, nonland card, draw a card.";

            p.Trigger(new WhenPlayerDiscardsCard((e, ctx) =>
              ctx.Opponent == e.Player && !e.Card.Is().Backup && !e.Card.Is().Forward));

            p.Effect = () => new DrawCards(1);

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}