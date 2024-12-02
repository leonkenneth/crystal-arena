using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.AI.TimingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Infrastructure;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;
using ReturnToHand = CrystalArena.Effects.ReturnToHand;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_024R_Shiva : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("23-024R")
            .Named("Shiva")
            .Cost(2, "I")
            .Category("FFL")
            .Summon()
            .Text(
                "{EX BURST} Choose 1 Forward. Dull it and Freeze it. During this turn, if it deals damage to a Forward or a player, the damage becomes 0 instead.")
            .Cast(p =>
            {
                p.Text = "Choose 1 Forward. Dull it and Freeze it. During this turn, if it deals damage to a Forward or a player, the damage becomes 0 instead.";

                p.Effect = () => new CompoundEffect(
                    new DullAndFreezeTargets(),
                    new PreventAllDamageFromSourceUntilEot());
                p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            });
    }
}