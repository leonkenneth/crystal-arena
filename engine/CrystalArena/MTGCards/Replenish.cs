namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using Effects;

    public class Replenish : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Replenish")
                .ManaCost("{3}{W}")
                .Type("Sorcery")
                .Text(
                    "Return all monster cards from your breakZone to the battlefield. (Auras with nothing to enchant remain in your breakZone.)"
                )
                .FlavorText(
                    "Treasures, trinkets, trash—the relics of the past are brought forth again."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new PutCardsFromBreakZoneToBattlefield(
                            c => c.Is().Monster,
                            eachPlayer: false
                        );

                    p.TimingRule(new WhenYourBreakZoneCountIs(c => c.Is().Monster, minCount: 2));
                });
        }
    }
}
