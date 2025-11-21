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

public class Opus23_022R_Aemo : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-022R")
            .Named("Aemo")
            .Cost(2, "I")
            .Category("PICTLOGICA · FFL")
            .Job("Girl from the Future")
            .Backup()
            .Text(
                "{T}, put Aemo into the Break Zone: Your opponent removes all their hand from the game face down. Your opponent can look at these removed cards at any time. At the end of the turn, your opponent adds them back to their hand. You can only use this ability during your turn."
            )
            .ActivatedAbility(p =>
            {
                p.Cost = new AggregateCost(new Tap(), new SacrificeThis());
                p.Effect = () => new OpponentRemoveTheirHandFromTheGameUntilEot();
            });
    }
}
