using System.Collections.Generic;
using System.Linq;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.AI.TimingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;
using ReturnToHand = CrystalArena.Effects.ReturnToHand;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_009H_ZornAndThorn : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-009H")
            .Named("Zorn & Thorn")
            .Cost(2, "R")
            .Category("IX")
            .Forward()
            .Power(5000)
            .Text(
                "When Zorn & Thorn enters the field or attacks, choose 1 Forward opponent controls. It gains \"If possible, this Forward must block.\" until the end of the turn.\nWhen Zorn & Thorn is put from the field into the Break Zone, you may search for 1 Monster of cost 2 or less and play it onto the field."
            )
            .TriggeredAbility(p =>
            {
                p.ExBurst();
                p.Text =
                    "When Zorn & Thorn enters the field or attacks, choose 1 Forward opponent controls. It gains \"If possible, this Forward must block.\" until the end of the turn.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Trigger(new WhenThisAttacks());

                p.TargetSelector.AddEffect(trg =>
                    trg.Is.Forward(ControlledBy.Opponent).On.Battlefield()
                );
                p.Effect = () =>
                    new ApplyModifiersToTargets(() =>
                        new AddSimpleAbility(Static.MustBlockIfPossible) { UntilEot = true }
                    );
            })
            .TriggeredAbility(p =>
            {
                p.Text =
                    "When Zorn & Thorn is put from the field into the Break Zone, you may search for 1 Monster of cost 2 or less and play it onto the field.";
                p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.BreakZone));

                p.Effect = () =>
                    new SearchMainDeckPutToZone(
                        zone: Zone.Battlefield,
                        validator: (card, ctx) => card.Is().Monster && card.ConvertedCost <= 2
                    );
            });
    }
}
