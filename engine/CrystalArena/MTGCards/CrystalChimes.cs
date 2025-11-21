namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class CrystalChimes : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Crystal Chimes")
                .ManaCost("{3}")
                .Type("Artifact")
                .Text(
                    "{3},{T}, Sacrifice Crystal Chimes: Return all monster cards from your breakZone to your hand."
                )
                .FlavorText(
                    "As Serra was to learn, the peace and sanctity of her realm were as fragile as glass."
                )
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{3},{T}, Sacrifice Crystal Chimes: Return all monster cards from your breakZone to your hand.";
                    p.Cost = new AggregateCost(
                        new PayMana(3.Colorless()),
                        new Tap(),
                        new Sacrifice()
                    );
                    p.Effect = () => new ReturnAllCardsInBreakZoneToHand(c => c.Is().Monster);
                    p.TimingRule(
                        new WhenYourBreakZoneCountIs(minCount: 2, selector: c => c.Is().Monster)
                    );
                });
        }
    }
}
