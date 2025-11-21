namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TimingRules;
    using Costs;
    using Effects;

    public class BraidwoodSextant : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Braidwood Sextant")
                .ManaCost("{1}")
                .Type("Artifact")
                .Text(
                    "{2},{T}, Sacrifice Braidwood Sextant: Search your library for a basic backup card, reveal that card, and put it into your hand. Then shuffle your library."
                )
                .OverrideScore(p =>
                {
                    p.Hand = Scores.BackupInHandCost - 20;
                    p.Battlefield = Scores.BackupInHandCost - 10;
                })
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{2},{T}, Sacrifice Braidwood Sextant: Search your library for a basic backup card, reveal that card, and put it into your hand. Then shuffle your library.";

                    p.Cost = new AggregateCost(
                        new PayMana(2.Colorless()),
                        new Tap(),
                        new Sacrifice()
                    );

                    p.Effect = () =>
                        new SearchMainDeckPutToZone(
                            zone: Zone.Hand,
                            minCount: 0,
                            maxCount: 1,
                            validator: (c, ctx) => c.Is().BasicBackup,
                            text: "Search your library for basic backup card.",
                            rankingAlgorithm: SearchMainDeckPutToZone.ChooseBackupToPutToBattlefield
                        );

                    p.TimingRule(new OnFirstMain());
                });
        }
    }
}
