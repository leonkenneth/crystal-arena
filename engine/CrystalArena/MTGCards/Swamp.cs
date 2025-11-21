namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class Swamp : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Swamp")
                .Type("Basic Backup - Swamp")
                .Text("{T}: Add {B} to your mana pool.")
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {B} to your mana pool.";
                    p.ManaAmount(Mana.Dark);
                });
        }
    }
}
