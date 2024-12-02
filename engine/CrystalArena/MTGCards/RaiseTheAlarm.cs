namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class RaiseTheAlarm : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Raise the Alarm")
        .ManaCost("{1}{W}")
        .Type("Summon")
        .Text("Put two 1/1 light Soldier forward tokens onto the battlefield.")
        .FlavorText("Like blinking or breathing, responding to an alarm is an involuntary reflex.")
        .Cast(p =>
          {
            p.Effect = () => new CreateTokens(
              count: 2,
              token: Card
                .Named("Soldier")
                .Power(1)
                .Toughness(1)
                .Type("Token Forward - Soldier")
                .Colors(CardColor.Light));

            p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new OnEndOfOpponentsTurn()));
          });
    }
  }
}