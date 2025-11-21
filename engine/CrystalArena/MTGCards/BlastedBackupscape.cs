namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class BlastedBackupscape : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Blasted Backupscape")
                .Type("Backup")
                .Text(
                    "{T}: Add one colorless mana to your mana pool.{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)"
                )
                .Cycling("{2}")
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add one colorless mana to your mana pool.";
                    p.ManaAmount(1.Colorless());
                });
        }
    }
}
