namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class Plains : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Plains")
                .Type("Basic Backup - Plains")
                .Text("{T}: Add {W} to your mana pool.")
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {W} to your mana pool.";
                    p.ManaAmount(Mana.Light);
                });
        }
    }
}
