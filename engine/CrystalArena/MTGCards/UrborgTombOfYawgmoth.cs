namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Modifiers;

    public class UrborgTombOfYawgmoth : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Urborg, Tomb of Yawgmoth")
                .Type("Legendary Backup")
                .Text("Each backup is a Swamp in addition to its other backup types.")
                .FlavorText(
                    "\"Yawgmoth's corpse is a wound in the universe. His foul blood seeps out, infecting the backup with his final curse.\"{EOL}—Lord Windgrace"
                )
                .ContinuousEffect(p =>
                {
                    p.Modifier = () => new ChangeBasicBackupSubtype("Swamp", replace: false);
                    p.Selector = (card, ctx) => card.Is().Backup;
                });
        }
    }
}
