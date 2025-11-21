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

public class Opus23_016R_Bahamut : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-016R")
            .Named("Bahamut")
            .Cost(5, "R")
            .Category("IX")
            .Summon()
            .Text(
                "Choose 1 Forward with 9000 power or less and up to 1 Forward in your opponent's Break Zone. Remove them from the game."
            )
            .Cast(p =>
            {
                p.ExBurst();
                p.Text =
                    "Choose 1 Forward with 9000 power or less and up to 1 Forward in your opponent's Break Zone. Remove them from the game.";
                p.Effect = () => new RemoveFromPlayTargets();
                p.TargetSelector.AddEffect(trg =>
                    trg.Is.ForwardWith(x => x.Power <= 9000).In.Battlefield()
                );
                p.TargetSelector.AddEffect(trg =>
                    trg.Is.Forward(ControlledBy.Opponent).In.BreakZone()
                );
            });
    }
}
