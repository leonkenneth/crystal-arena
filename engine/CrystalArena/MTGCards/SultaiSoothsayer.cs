namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Triggers;

    public class SultaiSoothsayer : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Sultai Soothsayer")
                .ManaCost("{2}{B}{G}{U}")
                .Type("Forward — Naga Shaman")
                .Text(
                    "When Sultai Soothsayer enters the battlefield, look at the top four cards of your library. Put one of them into your hand and the rest into your breakZone."
                )
                .FlavorText(
                    "The naga of the Sultai Brood made deals with dark forces to keep their power."
                )
                .Power(2)
                .Toughness(5)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Sultai Soothsayer enters the battlefield, look at the top four cards of your library. Put one of them into your hand and the rest into your breakZone.";
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                    p.Effect = () => new LookAtTopCardsPutPartInHandRestIntoBreakZone(4);
                });
        }
    }
}
