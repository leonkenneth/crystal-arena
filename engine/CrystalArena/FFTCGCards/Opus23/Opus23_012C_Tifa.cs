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

public class Opus23_012C_Tifa : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-012C")
            .Named("Tifa")
            .Cost(3, "R")
            .Categories("DFF", "VII")
            .Job("Martial Artist")
            .Forward()
            .Power(7000)
            .Text(
                "When a Forward damaged by Tifa is put from the field into the Break Zone on the same turn, gain {Z}.\nWhen Tifa attacks, choose 1 Forward. Deal it 2000 damage.\n{Z}: Until the end of the turn, Tifa gains +2000 power and Haste."
            )
            .TriggeredAbility(p =>
            {
                p.ExBurst();
                p.Text =
                    "When a Forward damaged by Tifa is put from the field into the Break Zone on the same turn, gain {Z}.";
                p.Trigger(new OnForwardDamagedBySelfDiesInSameTurn());
                p.Effect = () => new AddManaToPool("{Z}".Parse());
            })
            .TriggeredAbility(p =>
            {
                p.Text = "When Tifa attacks, choose 1 Forward. Deal it 2000 damage.";
                p.Trigger(new WhenThisAttacks());
                p.Effect = () => new DealDamageToTargets(2000);
                p.TargetSelector.AddEffect(
                    trg => trg.Is.Forward().On.Battlefield(),
                    trg =>
                    {
                        trg.Message = "Select Forward to deal damage to.";
                    }
                );
            })
            .ActivatedAbility(p =>
            {
                p.Text = "{Z}: Until the end of the turn, Tifa gains +2000 power and Haste.";
                p.Cost = new PayMana("{Z}".Parse());
                p.Effect = () =>
                    new ApplyModifiersToSelf(
                        () => new AddPowerAndToughness(+2000, +2000) { UntilEot = true },
                        () => new AddSimpleAbility(Static.Haste) { UntilEot = true }
                    );
                p.TimingRule(new PumpOwningCardTimingRule(2000, 2000));
            });
    }
}
