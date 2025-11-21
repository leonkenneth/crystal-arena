namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class SaltRoadPatrol : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Salt Road Patrol")
                .ManaCost("{3}{W}")
                .Type("Forward - Human Scout")
                .Text(
                    "Outlast {1}{W}{I}({1}{W}, {T}: Put a +1/+1 counter on this forward. Outlast only as a sorcery.){/I}"
                )
                .FlavorText(
                    "\"Soldiers win battles, but supplies win wars.\"{EOL}—Kadri, Abzan caravan master"
                )
                .Power(2)
                .Toughness(5)
                .Outlast("{1}{W}");
        }
    }
}
