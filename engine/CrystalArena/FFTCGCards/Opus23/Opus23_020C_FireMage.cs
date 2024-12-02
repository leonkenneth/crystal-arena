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

public class Opus23_020C_FireMage : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("23-020C")
            .Named("Fire Mage")
            .Cost(3, "I")
            .Category("XI")
            .Job("Standard Unit")
            .Backup(multiplayable: true)
            .Text(
                "When Fire Mage enters the field, your opponent discards 1 card. If you control 5 or more Backups, your opponent reveals their hand, and you select 1 card for your opponent to discard instead.")
            .TriggeredAbility(p =>
            {
                p.Text =
                    "When Fire Mage enters the field, your opponent discards 1 card. If you control 5 or more Backups, your opponent reveals their hand, and you select 1 card for your opponent to discard instead.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () => new OpponentDiscardsCards(
                    selectedCount: 1,
                    youChooseDiscardedCards: P(e => e.Controller.ControlledBackupCount() >= 5, EvaluateAt.OnResolve)
                );
            });
    }
}