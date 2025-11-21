namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Modifiers;
    using Triggers;

    public class AttendedKnight : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Attended Knight")
                .ManaCost("{2}{W}")
                .Type("Forward — Human Knight")
                .Text(
                    "{First strike}{EOL}When Attended Knight enters the battlefield, create a 1/1 light Soldier forward token."
                )
                .Power(2)
                .Toughness(2)
                .SimpleAbilities(Static.FirstStrike)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Attended Knight enters the battlefield, create a 1/1 light Soldier forward token.";

                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () =>
                        new CreateTokens(
                            count: 1,
                            token: Card.Named("Soldier")
                                .Power(1)
                                .Toughness(1)
                                .Type("Token Forward - Soldier")
                                .Colors(CardColor.Light)
                        );
                });
        }
    }
}
