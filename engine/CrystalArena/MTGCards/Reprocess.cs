namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Reprocess : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Reprocess")
                .ManaCost("{2}{B}{B}")
                .Type("Sorcery")
                .Text(
                    "Sacrifice any number of artifacts, forwards, and/or backups. Draw a card for each permanent sacrificed this way."
                )
                .FlavorText("Everything will find its use in Phyrexia. Eventually.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new DrawCardsEqualToSacrificedPermanentsCount(
                            text: "Sacrifice any number of artifacts, forwards, and/or backups.",
                            validator: c => c.Is().Backup || c.Is().Forward || c.Is().Artifact
                        );

                    p.TimingRule(new OnSecondMain());
                });
        }
    }
}
