using System.Collections.Generic;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_003C_Kain : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-003C")
            .Named("Kain")
            .Text(
                "Haste First Strike\nWhen Kain attacks, all the Forwards with Haste or First Strike you control gain +2000 power until the end of the turn.\nDouble Jump {S}: Choose 1 Forward. Deal it 8000 damage."
            )
            .Cost(4, "R")
            .Power(7000)
            .Toughness(7000)
            .Forward()
            .SimpleAbilities(Static.Haste, Static.FirstStrike)
            .TriggeredAbility(p =>
            {
                p.ExBurst();
                p.Text =
                    "When Kain attacks, all the Forwards with Haste or First Strike you control gain +2000 power until the end of the turn.";
                p.Trigger(new WhenThisAttacks());
                p.Effect = () =>
                    new ApplyModifiersToPermanents(
                        selector: (card, ctx) =>
                            card.Is().Forward
                            && (card.Has().Haste || card.Has().FirstStrike)
                            && card.Controller == ctx.OwningCard.Controller(),
                        modifier: () => new AddPowerAndToughness(+2000, +2000) { UntilEot = true }
                    );
            })
            .SpecialAbility(
                "Double Jump",
                p =>
                {
                    p.Text = "Double Jump {S}: Choose 1 Forward. Deal it 8000 damage.";
                    p.Effect = () => new DealDamageToTargets(8000);
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TargetingRule(new EffectDealDamage(8000));
                }
            );
    }
}
