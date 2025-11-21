using System.Collections.Generic;
using CrystalArena.AI;
using CrystalArena.AI.TargetingRules;
using CrystalArena.Costs;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;

namespace CrystalArena.FFTCGCards.Opus23;

public class Opus23_004R_Kefka : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("23-004R")
            .Named("Kefka")
            .Text(
                "When Kefka attacks, choose 1 Forward opponent controls. Deal it 4000 damage.\nDamage 5 -- Kefka gains +2000 power, Haste and \"If Kefka deals damage to a Forward or your opponent, double the damage instead.\""
            )
            .Cost(3, "R")
            .Forward()
            .Power(7000)
            .TriggeredAbility(p =>
            {
                p.ExBurst();
                p.Text =
                    "When Kefka attacks, choose 1 Forward opponent controls. Deal it 4000 damage.";
                p.Trigger(new WhenThisAttacks());
                p.Effect = () => new DealDamageToTargets(4000);
                p.TargetSelector.AddEffect(trg =>
                    trg.Is.Forward(ControlledBy.Opponent).On.Battlefield()
                );
            })
            .StaticAbility(p =>
            {
                p.Modifier(() => new AddSimpleAbility(Static.Haste));
                p.Modifier(() => new AddPowerAndToughness(+2000, +2000));
                p.Modifier(() =>
                {
                    return new AddDamageRedirection(modifier => new ReplaceDamage(
                        dmg => dmg.Source == modifier.SourceCard,
                        dmg => dmg.Amount *= 2
                    ));
                });
                p.Condition = cond => cond.YouHaveLessThanXLife(2);
            });
    }
}
