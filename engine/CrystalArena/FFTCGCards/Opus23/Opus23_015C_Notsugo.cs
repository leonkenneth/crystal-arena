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

public class Opus23_015C_Notsugo : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-015C")
            .Named("Notsugo")
            .Cost(1, "R")
            .Category("XIII")
            .Job("Spook")
            .Monster(multiplayable: true)
            .Text(
                "{T}, put Notsugo and 1 Monster into the Break Zone: Choose 1 Forward opponent controls. Deal it 9000 damage."
            )
            .ActivatedAbility(p =>
            {
                p.Cost = new AggregateCost(new Tap(), new SacrificeThis(), new SacrificeTarget());
                p.TargetSelector.AddCost(trg => trg.Is.Monster().In.Battlefield());
                p.Effect = () => new DealDamageToTargets(9000);
                p.TargetSelector.AddEffect(trg =>
                    trg.Is.Forward(ControlledBy.Opponent).In.Battlefield()
                );
            });
    }
}
