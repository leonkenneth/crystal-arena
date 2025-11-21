namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class GurmagAngler : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Gurmag Angler")
                .ManaCost("{6}{B}")
                .Type("Forward — Zombie Fish")
                .Text(
                    "{Delve}{I}(Each card you exile from your breakZone while casting this spell pays for {1}.){/I}"
                )
                .FlavorText(
                    "If everything in the Gurmag Swamp hungers for human flesh, what bait could be more effective?"
                )
                .Power(5)
                .Toughness(5)
                .SimpleAbilities(Static.Delve);
        }
    }
}
