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

public class Opus23_031C_SoldierCandidate : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-031C")
            .Named("SOLDIER Candidate")
            .Cost(2, "I")
            .Category("VII")
            .Job("Standard Unit")
            .Forward(multiplayable: true)
            .Power(5000)
            .Text(
                "If you have 2 or more Job Standard Unit Forwards in your Break Zone, SOLDIER Candidate gains +3000 power.\nWhen SOLDIER Candidate enters the field, choose 1 Forward opponent controls. Dull it and Freeze it."
            )
            .StaticAbility(p =>
            {
                p.Condition = parameters =>
                    parameters.OwnerHasCardInBreakZone(cards =>
                        cards.Count(x => x.Is().Forward && x.HasJob("Standard Unit")) >= 2
                    );
                p.Modifiers.Add(() => new AddPowerAndToughness(+3000, +3000));
            })
            .TriggeredAbility(p =>
            {
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () => new DullAndFreezeTargets();
                p.TargetSelector.AddEffect(trg =>
                    trg.Is.Forward(ControlledBy.Opponent).On.Battlefield()
                );
            });
    }
}
