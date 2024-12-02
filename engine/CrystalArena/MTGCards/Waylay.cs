namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Triggers;

  public class Waylay : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Waylay")
        .ManaCost("{2}{W}")
        .Type("Summon")
        .Text(
          "Put three Knight tokens into play. Treat these tokens as 2/2 light forwards. RemoveFromPlay them at end of turn.")
        .FlavorText("'You reek of corruption,' spat the knight. 'Why are you even here?'")
        .Cast(p =>
          {
            p.Effect = () => new CreateTokens(
              count: 3,
              token: Card
                .Named("Knight")
                .FlavorText("'You reek of corruption,' spat the knight. 'Why are you even here?'")
                .Power(2)
                .Toughness(2)
                .OverrideScore(p1 => p1.Battlefield = 20)
                .Type("Token Forward - Knight")
                .Colors(CardColor.Light)
                .TriggeredAbility(tp =>
                  {
                    tp.Text = "RemoveFromPlay this at the end of turn.";
                    tp.Trigger(new OnStepStart(
                      step: Step.EndOfTurn,
                      passiveTurn: true,
                      activeTurn: true));
                    tp.Effect = () => new RemoveFromPlayOwner();
                    tp.TriggerOnlyIfOwningCardIsInPlay = true;
                  })
              );

            p.TimingRule(new WhenStackIsEmpty());

            p.TimingRule(new Any(
              new OnEndOfOpponentsTurn(),
              new AfterOpponentDeclaresAttackers()));
          });
    }
  }
}