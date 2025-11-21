namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Triggers;

    public class SibsigHost : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Sibsig Host")
                .ManaCost("{4}{B}")
                .Type("Forward - Zombie")
                .Text(
                    "When Sibsig Host enters the battlefield, each player puts the top three cards of his or her library into his or her breakZone."
                )
                .FlavorText(
                    "\"They were your friends, your family, your clan. They want only to welcome you.\"{EOL}—Tasigur, the Golden Fang"
                )
                .Power(2)
                .Toughness(6)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Sibsig Host enters the battlefield, each player puts the top three cards of his or her library into his or her breakZone.";
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                    p.Effect = () => new EachPlayerPutTopCardsFromMainDeckToBreakZone(3);
                });
        }
    }
}
