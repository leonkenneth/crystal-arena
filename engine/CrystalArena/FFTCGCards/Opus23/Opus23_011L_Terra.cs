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

public class Opus23_011L_Terra : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-011L")
            .Named("Terra")
            .Cost(5, "R")
            .Category("VI")
            .Job("Magitek Knight")
            .Forward()
            .Power(5000)
            .Toughness(5000)
            .Text(
                "{EX BURST} When Terra enters the field or is put from the field into the Break Zone, choose 1 Summon in your Break Zone. Add it to your hand. During this turn, the cost required to cast your next Summon is reduced by 2 (it cannot become 0).\nAll Summons in your Break Zone cannot be removed from the game by your opponent's Summons or abilities."
            )
            .TriggeredAbility(p =>
            {
                p.Text =
                    "When Terra enters the field or is put from the field into the Break Zone, choose 1 Summon in your Break Zone. Add it to your hand. During this turn, the cost required to cast your next Summon is reduced by 2 (it cannot become 0).";
                p.ExBurst();
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.BreakZone));
                p.Effect = () =>
                    new CompoundEffect(
                        new ChooseInBreakZonePutToZone(
                            Zone.Hand,
                            validator: (card, ctx) => card.Is().Summon
                        ),
                        new ApplyModifiersToGame(() =>
                            new AddCostModifier(
                                new ChangeManaCostOfSpellsOrAbilities(
                                    2,
                                    (card, costType, modifier) =>
                                        card.Is().Summon && costType == CostType.Spell
                                )
                            )
                            {
                                UntilEot = true,
                            }
                        )
                    );
            })
            .ContinuousEffect(
                p =>
                {
                    p.Selector = (card, ctx) => card.Zone() == Zone.BreakZone && card.Is().Summon;
                    p.Modifier = () => new AddProtectionFromOpponentRemoveFromGameEffects();
                    p.ApplyOnlyToPermanents = false;
                },
                true
            );
    }
}
