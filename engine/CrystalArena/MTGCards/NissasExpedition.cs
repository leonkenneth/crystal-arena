namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;

    public class NissasExpedition : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Nissa's Expedition")
                .ManaCost("{4}{G}")
                .Type("Sorcery")
                .Text(
                    "{Convoke}{I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}Search your library for up to two basic backup cards, put them onto the battlefield tapped, then shuffle your library."
                )
                .SimpleAbilities(Static.Convoke)
                .Cast(p =>
                {
                    p.Effect = () =>
                        new SearchMainDeckPutToZone(
                            zone: Zone.Battlefield,
                            afterPutToZone: (c, g) => c.Tap(),
                            minCount: 0,
                            maxCount: 2,
                            validator: (c, ctx) => c.Is().BasicBackup,
                            text: "Search your library for up to two basic backup cards.",
                            rankingAlgorithm: SearchMainDeckPutToZone.ChooseBackupToPutToBattlefield
                        );
                });
        }
    }
}
