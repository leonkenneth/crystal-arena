namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class GaeasCradle : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Gaea's Cradle")
                .Type("Legendary Backup")
                .Text("{T}: Add {G} to your mana pool for each forward you control.")
                .FlavorText(
                    "Here sprouted the first seedling of Argoth. Here the last tree will fall."
                )
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {G} to your mana pool for each forward you control.";
                    p.ManaAmount(ManaColor.Wind, c => c.Is().Forward);
                });
        }
    }
}
