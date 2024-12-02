using System.Collections.Generic;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_001C_Garland : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("23-001C")
            .Named("Garland")
            .Text(
                "When Garland enters the field, choose 1 Forward opponent controls. You gain control of it until the end of the turn.")
            .ManaCost("{1}{R}")
            .Backup()
            .TriggeredAbility(p =>
            {
                p.Text =
                    "When Garland enters the field, choose 1 Forward opponent controls. You gain control of it until the end of the turn.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () =>
                    new Attach(() => new ChangeController(m => m.SourceCard.Controller) { UntilEot = true })
                        .SetTags(EffectTag.ChangeController);
                p.TargetSelector.AddEffect(trg => trg.Is.Forward(ControlledBy.Opponent).On.Battlefield());
                p.TargetingRule(new EffectGainControl());
            });
    }
}