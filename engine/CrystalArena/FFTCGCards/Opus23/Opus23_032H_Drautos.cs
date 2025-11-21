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

public class Opus23_032H_Drautos : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-032H")
            .Named("Drautos")
            .Cost(2, "I")
            .Category("XV")
            .Job("Kingsglaive")
            .Forward()
            .Power(8000)
            .Text(
                "You can only cast Drautos if your opponent has 2 cards or less in their hand.\nBrave\nIf Drautos is dealt damage by your opponent's Summons or abilities, the damage becomes 0 instead."
            )
            .Cast(p =>
            {
                p.Condition = (card, game) => card.Controller.Opponent.Hand.Count <= 2;
            })
            .SimpleAbilities(Static.Brave)
            .StaticAbility(p =>
            {
                p.Modifier(() =>
                    new AddDamagePrevention(
                        (m) =>
                            new PreventDamageToTarget(
                                target: m.SourceCard,
                                sourceSelector: (card, context) =>
                                    card.Controller == m.SourceCard.Controller.Opponent,
                                preventCombatDamage: false
                            )
                    )
                );
            });
    }
}
