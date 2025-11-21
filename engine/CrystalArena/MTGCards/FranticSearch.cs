namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class FranticSearch : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Frantic Search")
                .ManaCost("{2}{U}")
                .Type("Summon")
                .Text("Draw two cards, then discard two cards. Untap up to three backups.")
                .FlavorText(
                    "Motivation was high in the academy once students realized flunking their exams could kill them."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CompoundEffect(
                            new DrawCards(count: 2, discardCount: 2),
                            new UntapSelectedPermanents(
                                minCount: 0,
                                maxCount: 3,
                                validator: c => c.Is().Backup,
                                text: "Select backups to untap."
                            )
                        );

                    p.TimingRule(new OnYourTurn(Step.FirstMain));
                });
        }
    }
}
