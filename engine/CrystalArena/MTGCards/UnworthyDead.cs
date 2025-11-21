namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class UnworthyDead : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Unworthy Dead")
                .ManaCost("{1}{B}")
                .Type("Forward Skeleton")
                .Text("{B}: Regenerate Unworthy Dead.")
                .FlavorText(
                    "Great Yawgmoth moves across the seas of shard and bone and rust. We exalt him in life, in death, and in between."
                )
                .Power(1)
                .Toughness(1)
                .Regenerate(cost: Mana.Dark, text: "{B}: Regenerate Unworthy Dead.");
        }
    }
}
