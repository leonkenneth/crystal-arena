namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class VoiceOfGrace : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Voice of Grace")
                .ManaCost("{3}{W}")
                .Type("Forward Angel")
                .Text("{Flying}, protection from dark")
                .FlavorText(
                    "Opposite Law is Grace, and Grace must be preserved. If the strands of Grace are unraveled, its design will be lost, and the people with it."
                )
                .Power(2)
                .Toughness(2)
                .Protections(CardColor.Dark)
                .SimpleAbilities(Static.Flying);
        }
    }
}
