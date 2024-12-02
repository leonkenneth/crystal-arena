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

public class Opus23_029R_Zenos : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card
            .Code("23-029R")
            .Named("Zenos")
            .Cost(4, "I")
            .Category("XIV")
            .Job("Reaper")
            .Forward()
            .Power(7000)
            .Text(
                "When Zenos enters the field, choose up to 2 Forwards in your opponent's Break Zone. Remove them from the game.\nWhen a card in your opponent's Break Zone leaves the Break Zone, your opponent discards 1 card. This effect will trigger only once per turn.")
            .TriggeredAbility(p =>
            {
                p.Text =
                    "When Zenos enters the field, choose up to 2 Forwards in your opponent's Break Zone. Remove them from the game.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () => new RemoveFromPlayTargets();
                p.TargetSelector.AddEffect(trg => trg.Is.Forward().In.BreakZone(), cfg => cfg.MaxCount = 2);
            })
            .TriggeredAbility(p =>
            {
                p.Text = "When a card in your opponent's Break Zone leaves the Break Zone, your opponent discards 1 card. This effect will trigger only once per turn.";
                p.Trigger(new OnZoneChanged(from: Zone.BreakZone, selector: (card, ctx) => card.Controller == ctx.OwningCard.Controller().Opponent));
                p.Effect = () => new OpponentDiscardsCards(selectedCount: 1);
                p.TriggerOnlyOncePerTurn = true;
                p.TriggerOnlyIfOwningCardIsInPlay = true;
            });
    }
}