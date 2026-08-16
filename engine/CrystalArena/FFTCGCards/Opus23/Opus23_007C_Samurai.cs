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

public class Opus23_007C_Samurai : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-007C")
            .Named("Samurai")
            .Cost(2, "R")
            .Backup(multiplayable: true)
            .Job("Standard Unit")
            .Text(
                "When Samurai enters the field, gain {Z}.\n{T}, put Samurai into the Break Zone: Choose 1 Forward. If it deals damage to a Forward this turn, the damage increases by 1000 instead. You can only use this ability during your turn."
            )
            .TriggeredAbility(p =>
            {
                p.ExBurst();
                p.Text = "When Samurai enters the field, gain {Z}.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                p.Effect = () => new AddManaToPool("{Z}".Parse());
            })
            .ActivatedAbility(p =>
            {
                p.Text =
                    "{T}, put Samurai into the Break Zone: Choose 1 Forward. If it deals damage to a Forward this turn, the damage increases by 1000 instead.";
                p.Cost = new AggregateCost(new Tap(), new Sacrifice());
                p.Effect = () =>
                    new ReplaceDamageToTargets(
                        (target) =>
                            new ReplaceDamage(
                                damage =>
                                    damage.Source == target
                                    && damage.Target.IsPermanent()
                                    && damage.Target.Permanent().Is().Forward,
                                dmg => dmg.Amount += 1000,
                                hashDependency: target
                            ),
                        true
                    );
                p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                p.ActivateOnlyDuringYourTurn = true;
            });
        ;
    }
}
