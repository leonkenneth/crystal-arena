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

public class Opus23_006R_Soulcage : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-006R")
            .Named("Soulcage")
            .Cost(2, "R")
            .Monster()
            .Text(
                "If you control 2 or more Monsters, Soulcage also becomes a Forward with 9000 power.\nWhen Soulcage is put from the field into the Break Zone, you may remove Soulcage from the game. When you do so, choose 1 Monster in your Break Zone. Add it to your hand."
            )
            .StaticAbility(p =>
            {
                p.Condition = (cond) =>
                    cond.OwnerControlsPermanents(permanents =>
                        permanents.Count(x => x.Is().Monster) >= 2
                    );
                p.Modifier(() =>
                    new ChangeToForward(
                        power: 9000,
                        toughness: 9000,
                        type: t => t.Change(baseTypes: "forward monster")
                    )
                );
            })
            .TriggeredAbility(p =>
            {
                p.ExBurst();
                p.Text =
                    "When Soulcage is put from the field into the Break Zone, you may remove Soulcage from the game. When you do so, choose 1 Monster in your Break Zone. Add it to your hand.";
                p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.BreakZone));

                p.Effect = () =>
                    new CompoundEffect(
                        new RemoveFromPlayOwner(),
                        new ChooseInBreakZonePutToZone(
                            zone: Zone.Hand,
                            minCount: 0,
                            maxCount: 1,
                            validator: (c, ctx) => c.Is().Monster,
                            text: "Choose 1 Monster in your Break Zone."
                        )
                    );
            });
        ;
    }
}
