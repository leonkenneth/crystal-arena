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

public class Opus23_021C_Weiss : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-021C")
            .Named("Weiss")
            .Cost(3, "I")
            .Category("DFF · VII")
            .Job("Tsviets")
            .Forward()
            .Power(7000)
            .Text(
                "If you control 2 or more Job Tsviets, Weiss gains +2000 power and Brave.\nWhen Weiss enters the field, you may pay {Z}. When you do so, search for 1 Job Tsviets and add it to your hand."
            )
            .StaticAbility(p =>
            {
                p.Condition = cp =>
                    cp.OwnerControlsPermanents(cards => cards.Count(x => x.HasJob("Tsviets")) >= 2);
                p.Modifiers.Add(() => new AddPowerAndToughness(+2000, +2000));
                p.Modifiers.Add(() => new AddSimpleAbility(Static.Brave));
            })
            .TriggeredAbility(p =>
            {
                p.Text =
                    "When Weiss enters the field, you may pay {Z}. When you do so, search for 1 Job Tsviets and add it to your hand.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () =>
                    new PayManaThen(
                        "{Z}".Parse(),
                        new SearchMainDeckPutToZone(
                            zone: Zone.Hand,
                            validator: (card, ctx) => card.HasJob("Tsviets")
                        )
                    );
            });
    }
}
