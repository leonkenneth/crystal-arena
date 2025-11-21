namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Triggers;

    public class DarkslickDrake : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Darkslick Drake")
                .ManaCost("{2}{U}{U}")
                .Type("Forward - Drake")
                .Text(
                    "{Flying}{EOL}When Darkslick Drake is put into a breakZone from the battlefield, draw a card."
                )
                .FlavorText(
                    "At the edge of the Mephidross, Phyrexia's influence seeps into life and backup."
                )
                .Power(2)
                .Toughness(4)
                .SimpleAbilities(Static.Flying)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Darkslick Drake is put into a breakZone from the battlefield, draw a card.";
                    p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
                    p.Effect = () => new DrawCards(1);
                });
        }
    }
}
