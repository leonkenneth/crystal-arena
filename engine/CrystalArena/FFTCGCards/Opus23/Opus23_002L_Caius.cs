using System.Collections.Generic;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_002L_Caius : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-002L")
            .Named("Caius")
            .Text(
                "Brave\nWhen Caius enters the field, you may search for 1 Card Name Bahamut and add it to your hand. During this turn, the cost required to cast your next Card Name Bahamut is reduced by 5.\nWhen Caius is put from the field into the Break Zone, you may discard 3 cards. When you do so, play Caius from the Break Zone onto the field dull."
            )
            .Cost(5, "R")
            .Forward()
            .Power(9000)
            .TriggeredAbility(p =>
            {
                p.ExBurst();
                p.Text =
                    "When Caius enters the field, you may search for 1 Card Name Bahamut and add it to your hand. During this turn, the cost required to cast your next Card Name Bahamut is reduced by 5.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () =>
                    new CompoundEffect(
                        new SearchMainDeckPutToZone(
                            zone: Zone.Hand,
                            minCount: 0,
                            maxCount: 1,
                            validator: (c, ctx) => c.Name == "Bahamut",
                            text: "Search for 1 Card Name Bahamut."
                        ),
                        new ApplyModifiersToGame(() =>
                            new AddCostModifier(
                                new ChangeManaCostOfSpellsOrAbilities(
                                    -5,
                                    (card, costType, modifier) =>
                                        card.Name == "Bahamut" && costType == CostType.Spell
                                )
                            )
                        )
                    );
            });
    }
}
