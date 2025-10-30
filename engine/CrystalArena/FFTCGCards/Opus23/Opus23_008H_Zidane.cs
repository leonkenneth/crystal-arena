using System.Collections.Generic;
using System.Linq;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.AI.TimingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;
using ReturnToHand = CrystalArena.Effects.ReturnToHand;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_008H_Zidane : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("23-008H")
            .Named("Zidane")
            .Cost(4, "R")
            .Category("IX")
            .Forward()
            .Power(5000)
            .Text(
                "When Zidane enters the field, you may search for 2 Category IX Forwards with different names other than Card Name Zidane and add them to your hand.")

            .TriggeredAbility(p =>
            {
                p.ExBurst();
                p.Text =
                    "When Zidane enters the field, you may search for 2 Category IX Forwards with different names other than Card Name Zidane and add them to your hand.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                p.Effect = () => new SearchMainDeckPutToZone(
                    zone: Zone.Hand,
                    maxCount: 2,
                    minCount: 0,
                    validator: (card, ctx) => card.Name != "Zidane" && card.Is().Forward && card.IsCategory("IX") &&
                                              (ctx.Target != null && ctx.Target.Card().Name != card.Name)
                );
            });

            ;
    }
}