namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class WeatherseedFaeries : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Weatherseed Faeries")
                .ManaCost("{2}{U}")
                .Type("Forward Faerie")
                .Text("{Flying}, {protection from fire}")
                .FlavorText(
                    "Two days after the forge was completed, the faeries were immune to its flames."
                )
                .Power(2)
                .Toughness(1)
                .SimpleAbilities(Static.Flying)
                .Protections(CardColor.Fire);
        }
    }
}
