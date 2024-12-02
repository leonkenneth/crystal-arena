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

public class Opus23_028L_Cecil : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("23-028L")
            .Named("Cecil")
            .Cost(3, "I")
            .Category("IV")
            .Job("Dark Knight")
            .Forward()
            .Power(9000)
            .Text(
                "When Cecil enters the field, discard 1 card.\nWhen Cecil enters the field, you may receive 1 point of damage. When you do so, choose 2 Characters. Dull them and Freeze them.\nDamage 5 -- Dark Flame {S}: Deal 10000 damage to all the Forwards opponent controls. Cecil deals you 1 point of damage.")
            .TriggeredAbility(p =>
            {
                p.Text = "When Cecil enters the field, discard 1 card.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () => new DiscardCards(1, P(e => e.Controller));
            }).TriggeredAbility(p =>
            {
                p.Text =
                    "When Cecil enters the field, you may receive 1 point of damage. When you do so, choose 2 Characters. Dull them and Freeze them.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () => new PayLifeThen(1, new DullAndFreezeTargets());
                p.TargetSelector.AddEffect(trg => trg.Is.Card().On.Battlefield(), cfg => cfg.MaxCount = 2);
            })
            .Damage(5, sap =>
            {
                sap.Modifiers.Add(() =>
                {
                    var p = new ActivatedAbilityParameters()
                    {
                        Text =
                            "Dark Flame {S}: Deal 10000 damage to all the Forwards opponent controls. Cecil deals you 1 point of damage.",
                        Effect = () => new CompoundEffect(new DealDamageToForwardsAndPlayers(
                            amountForward: 1000,
                            amountPlayer: 0,
                            filterForward: (effect, card) =>
                                card.Is().Forward && card.Controller == effect.Controller.Opponent
                        ), new DealDamageToPlayer(1, P(e => e.Controller)))
                    };
                    p.TargetSelector.AddCost(t => t.Is.Card(c => c.Name == "Cecil").In.OwnersHand());
                    var activatedAbility = new ActivatedAbility(p);
                    return new AddActivatedAbility(activatedAbility);
                });
            });
    }
}