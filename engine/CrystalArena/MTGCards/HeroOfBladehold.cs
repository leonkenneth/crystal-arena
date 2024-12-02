namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;

  public class HeroOfBladehold : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hero of Bladehold")
        .ManaCost("{2}{W}{W}")
        .Type("Forward Human Knight")
        .Text(
          "{Battle cry}(Whenever this forward attacks, each other attacking forward gets +1/+0 until end of turn.){EOL}Whenever Hero of Bladehold attacks, put two 1/1 light Soldier forward tokens onto the battlefield tapped and attacking.")
        .Power(3)
        .Toughness(4)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever this forward attacks, each other attacking forward gets +1/+0 until end of turn.";
            p.Trigger(new WhenThisAttacks());
            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (c, ctx) => ctx.OwningCard != c && c.IsAttacker,
              modifier: () => new AddPowerAndToughness(1, 0) {UntilEot = true});
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Hero of Bladehold attacks, put two 1/1 light Soldier forward tokens onto the battlefield tapped and attacking.";

            p.Trigger(new WhenThisAttacks());
            p.Effect = () => new CreateTokens(
              count: 2,
              token: Card
                .Named("Soldier")
                .FlavorText(
                  "If you need an example to lead others to the front lines, consider the precedent set.")
                .Power(1)
                .Toughness(1)
                .Type("Token Forward - Soldier")
                .Colors(CardColor.Light),
              afterTokenComesToPlay: (token, ctx) => ctx.Combat.AddAttacker(token));
          });
    }
  }
}