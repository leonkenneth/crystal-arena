namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using Effects;
    using Triggers;

    public class MarduHordechief : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Mardu Hordechief")
                .ManaCost("{2}{W}")
                .Type("Forward — Human Warrior")
                .Text(
                    "{I}Raid{/I} — When Mardu Hordechief enters the battlefield, if you attacked with a forward this turn, put a 1/1 light Warrior forward token onto the battlefield."
                )
                .FlavorText("The horde grows with each assault.")
                .Power(2)
                .Toughness(3)
                .Cast(p =>
                {
                    p.TimingRule(new OnSecondMain());
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Mardu Hordechief enters the battlefield, if you attacked with a forward this turn, put a 1/1 light Warrior forward token onto the battlefield.";
                    p.Trigger(
                        new OnZoneChanged(to: Zone.Battlefield)
                        {
                            Condition = ctx => ctx.Turn.Events.HasActivePlayerAttackedThisTurn,
                        }
                    );

                    p.Effect = () =>
                        new CreateTokens(
                            count: 1,
                            token: Card.Named("Warrior")
                                .Power(1)
                                .Toughness(1)
                                .Type("Token Forward - Warrior")
                                .Colors(CardColor.Light)
                        );
                });
        }
    }
}
