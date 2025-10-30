using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.AI.TimingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;
using ReturnToHand = CrystalArena.Effects.ReturnToHand;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_017C_Parai : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("23-017C")
            .Named("Parai")
            .Cost(2, "R")
            .Category("PICTLOGICA · FFL")
            .Job("Warrior")
            .Forward()
            .Power(5000)
            .Text(
                "When Parai enters the field, you may receive 1 point of damage. When you do so, search for 1 Category FFL Character and add it to your hand.")
            .TriggeredAbility(p =>
            {
                p.ExBurst();
                p.Text =
                    "When Parai enters the field, you may receive 1 point of damage. When you do so, search for 1 Category FFL Character and add it to your hand.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () => new PayLifeThen(1,
                    new SearchMainDeckPutToZone(Zone.Hand,
                        validator: (card, ctx) => card.IsCategory("FFL") && card.Is().Character));
            });
    }
}