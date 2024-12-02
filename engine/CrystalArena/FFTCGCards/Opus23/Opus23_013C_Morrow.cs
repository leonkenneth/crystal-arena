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

public class Opus23_013C_Morrow : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("23-013C")
            .Named("Morrow")
            .Cost(1, "R")
            .Categories("PICTLOGICA", "FFL")
            .Job("Adventurer-in-Training")
            .Backup()
            .Text(
                "Damage 5 -- When Morrow enters the field, choose 1 Forward. Deal it 9000 damage.")
            .Damage(5, p =>
            {
                var tp = new TriggeredAbility.Parameters()
                {
                    Text = "When Morrow enters the field, choose 1 Forward. Deal it 9000 damage.",
                    Effect = () => new DealDamageToTargets(9000)
                };
                tp.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                tp.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                p.Modifiers.Add(() => new AddTriggeredAbility(new TriggeredAbility(tp)));
            });
    }
}