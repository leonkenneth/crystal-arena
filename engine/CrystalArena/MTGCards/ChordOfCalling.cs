namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.CostRules;
    using AI.TimingRules;
    using Effects;

    public class ChordOfCalling : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Chord of Calling")
                .ManaCost("{G}{G}{G}")
                .HasXInCost()
                .Type("Summon")
                .Text(
                    "{Convoke} {I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}Search your library for a forward card with converted mana cost X or less and put it onto the battlefield. Then shuffle your library."
                )
                .SimpleAbilities(Static.Convoke)
                .Cast(p =>
                {
                    p.Effect = () =>
                        new SearchMainDeckPutToZone(
                            zone: Zone.Battlefield,
                            minCount: 0,
                            maxCount: 1,
                            validator: (c, ctx) => c.Is().Forward && c.ConvertedCost <= ctx.X,
                            text: "Search you library for a forward card."
                        );

                    p.TimingRule(new OnEndOfOpponentsTurn());
                    p.CostRule(new XIsMaxCostInYourMainDeck(c => c.Is().Forward));
                });
        }
    }
}
