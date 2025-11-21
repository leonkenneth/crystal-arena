namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class PlanarBirth : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Planar Birth")
                .ManaCost("{1}{W}")
                .Type("Sorcery")
                .Text(
                    "Return all basic backup cards from all breakZones to the battlefield tapped under their owners' control."
                )
                .FlavorText(
                    "From womb of nothingness sprang this place of beauty, purity, and hope realized."
                )
                .Cast(p =>
                {
                    p.TimingRule(new OnSecondMain());

                    p.Effect = () =>
                        new PutCardsFromBreakZoneToBattlefield(
                            c => c.Is().BasicBackup,
                            c => c.Tap(),
                            eachPlayer: true
                        );
                });
        }
    }
}
