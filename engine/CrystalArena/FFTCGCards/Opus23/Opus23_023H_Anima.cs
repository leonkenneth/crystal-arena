using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.AI.TimingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Infrastructure;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;
using ReturnToHand = CrystalArena.Effects.ReturnToHand;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_023H_Anima : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("23-023H")
            .Named("Anima, Eikon of Eikons")
            .Cost(5, "I")
            .Category("XIV")
            .Job("Primal")
            .Forward()
            .Power(9000)
            .Text(
                "When Anima, Eikon of Eikons enters the field, if both you and your opponent have no cards in hand, dull and Freeze all the Characters opponent controls.")
            .TriggeredAbility(p =>
            {
                p.Text = "When Anima, Eikon of Eikons enters the field, if both you and your opponent have no cards in hand, dull and Freeze all the Characters opponent controls.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
                {
                    Condition = (ctx => ctx.Players.All(player => player.Hand.None()))
                });
                p.Effect = () => new DullAndFreezePermanents(ctx => ctx.Opponent.Battlefield.ToArray());
            });
    }
}