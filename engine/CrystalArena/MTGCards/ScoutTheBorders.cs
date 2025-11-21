namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;

    public class ScoutTheBorders : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Scout the Borders")
                .ManaCost("{2}{G}")
                .Type("Sorcery")
                .Text(
                    "Reveal the top five cards of your library. You may put a forward or backup card from among them into your hand. Put the rest into your breakZone."
                )
                .FlavorText(
                    "\"I am in my element: the element of surprise.\"{EOL}—Mogai, Sultai scout"
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new RevealTopCardsPutOneInHandOthersIntoBreakZone(
                            5,
                            c => c.Is().Backup || c.Is().Forward
                        );
                });
        }
    }
}
