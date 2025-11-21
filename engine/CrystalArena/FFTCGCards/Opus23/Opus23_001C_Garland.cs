using System.Collections.Generic;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_001C_Garland : CardTemplateSource
{
    /*
    Rarity	Common
    Set	Opus XXIII (Hidden Trials)
    Element
    Fire
    Type	Backup
    Cost	2
    Power
    Job	Knight
    Categories
    PICTLOGICA
    I
    EX Burst	no
    Multiplayable	no
    Limit Break	no
    Abilities
    When Garland enters the field, choose 1 Forward opponent controls. You gain control of it until the end of the turn.
    */
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-001C")
            .Named("Garland")
            .Text(
                "When Garland enters the field, choose 1 Forward opponent controls. You gain control of it until the end of the turn."
            )
            .Cost(2, "R")
            .Backup()
            .Job("Knight")
            .Categories("PICTLOGICA", "I")
            .TriggeredAbility(p =>
            {
                p.ExBurst();
                p.Text =
                    "When Garland enters the field, choose 1 Forward opponent controls. You gain control of it until the end of the turn.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () =>
                    new Attach(() =>
                        new ChangeController(m => m.SourceCard.Controller) { UntilEot = true }
                    ).SetTags(EffectTag.ChangeController);
                p.TargetSelector.AddEffect(trg =>
                    trg.Is.Forward(ControlledBy.Opponent).On.Battlefield()
                );
                p.TargetingRule(new EffectGainControl());
            });
    }
}
