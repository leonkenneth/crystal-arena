namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class Mountain : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Mountain")
                .Type("Basic Backup - Mountain")
                .Text("{T}: Add {R} to your mana pool.")
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {R} to your mana pool.";
                    p.ManaAmount(Mana.Fire);
                });
        }
    }
}
