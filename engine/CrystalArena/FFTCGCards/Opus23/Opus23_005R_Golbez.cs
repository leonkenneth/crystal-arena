using System.Collections.Generic;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.AI.TimingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;
using ReturnToHand = CrystalArena.Effects.ReturnToHand;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_005R_Golbez : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("23-005R")
            .Named("Golbez")
            .Cost(5, "R")
            .Forward()
            .Power(9000)
            .Text(
                "{T}: Deal 5000 damage to all the Forwards opponent controls.\n{R}{T}: Choose 1 Forward in your Break Zone. Add it to your hand. You can only use this ability once per turn.\n{R}{R}{1}{T}: Choose 1 Forward. Break it.")
            
            .ActivatedAbility(p =>
            {
                p.Text = "{T}: Deal 5000 damage to all the Forwards opponent controls.";
                p.Cost = new Tap();
                p.Effect = () => new DealDamageToForwardsAndPlayers(amountForward: 5000,
                    filterForward: (effect, forward) => forward.Controller != effect.Source.OwningCard.Controller);
            })
            .ActivatedAbility(p =>
            {
                p.Text =
                    "{R}{T}: Choose 1 Forward in your Break Zone. Add it to your hand. You can only use this ability once per turn.";
                p.Cost = new AggregateCost(new PayMana("{R}".Parse()), new Tap());
                p.Effect = () => new ReturnToHand();
                p.TargetSelector.AddEffect((target) => target.Is.Forward().In.BreakZone());
                p.ActivateOnlyOnceEachTurn = true;
            })
            .ActivatedAbility(p =>
            {
                p.Text =
                    "{R}{R}{1}{T}: Choose 1 Forward. Break it.";
                p.Cost = new AggregateCost(new PayMana("{R}{R}{1}".Parse()), new Tap());
                p.Effect = () => new DestroyTargetPermanents();
                p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                p.TargetingRule(new EffectDestroy());
                p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Destroy));
            });
    }
}