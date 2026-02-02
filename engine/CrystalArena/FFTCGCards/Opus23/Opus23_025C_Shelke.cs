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

public class Opus23_025C_Shelke : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-025C")
            .Named("Shelke")
            .Cost(2, "I")
            .Category("PICTLOGICA · VII")
            .Job("Tsviets")
            .Forward()
            .Power(5000)
            .Text(
                "When Shelke or a Job Tsviets enters your field, gain {Z}. This effect will trigger only once per turn.\n{Z}: Shelke gains +3000 power until the end of the turn."
            )
            .TriggeredAbility(p =>
            {
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Trigger(
                    new OnZoneChanged(
                        to: Zone.Battlefield,
                        selector: (
                            (card, ctx) =>
                                card.HasJob("Tsviets") && card.Owner == ctx.OwningCard.Owner
                        )
                    )
                );
                p.Effect = () => new AddManaToPool("{Z}".Parse());
                p.TriggerOnlyOncePerTurn = true;
                p.TriggerOnlyIfOwningCardIsInPlay = true;
            })
            .ActivatedAbility(p =>
            {
                p.Cost = new PayMana("{Z}".Parse());
                p.Effect = () =>
                    new ApplyModifiersToSelf(() =>
                        new AddPowerAndToughness(+3000, +3000) { UntilEot = true }
                    );
            });
    }
}
