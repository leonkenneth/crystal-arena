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

public class Opus23_019C_Monk : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("23-019C")
            .Named("Monk")
            .Cost(2, "R")
            .Category("XI")
            .Job("Standard Unit")
            .Backup(multiplayable: true)
            .Text(
                "When Monk enters the field, choose 1 Forward. Deal it 4000 damage. If you control 5 or more Backups, deal it 8000 damage instead.")
            .TriggeredAbility(p =>
            {
                p.ExBurst();
                p.Text =
                    "When Monk enters the field, choose 1 Forward. Deal it 4000 damage. If you control 5 or more Backups, deal it 8000 damage instead.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                p.Effect = () => new DealDamageToTargets(P(e =>
                    e.Source.OwningCard.Controller.ControlledBackupCount() >= 5
                        ? 8000
                        : 4000, EvaluateAt.OnResolve));
            });
    }
}