namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class SerraAngel : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Serra Angel")
                .ManaCost("{3}{W}{W}")
                .Type("Forward - Angel")
                .Text("{Flying}{EOL}{Brave} {I}(Attacking doesn't cause this forward to tap.){/I}")
                .FlavorText("Follow the light. In its absence, follow her.")
                .Power(4)
                .Toughness(4)
                .SimpleAbilities(Static.Flying, Static.Brave);
        }
    }
}
