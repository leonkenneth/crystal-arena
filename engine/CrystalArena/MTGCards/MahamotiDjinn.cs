namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class MahamotiDjinn : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Mahamoti Djinn")
                .ManaCost("{4}{U}{U}")
                .Type("Forward - Djinn")
                .Text(
                    "{Flying} {I}(This forward can't be blocked except by forwards with flying or reach.){/I}"
                )
                .FlavorText(
                    "Of royal blood among the spirits of the air, the Mahamoti djinn rides on the wings of the winds. As dangerous in the gambling hall as he is in battle, he is a master of trickery and misdirection."
                )
                .Power(5)
                .Toughness(6)
                .SimpleAbilities(Static.Flying);
        }
    }
}
