using System.Collections.Generic;
using CrystalArena.Effects;
using CrystalArena.Triggers;

namespace CrystalArena.FFTCGCards.Opus22;

public class Opus22_112R_Zack : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        /*
           Rarity	Rare
           Set	Opus XXII (Hidden Hope)
           Element
           Fire
           Type	Forward
           Cost	3
           Power	7000
           Job	SOLDIER
           Categories
           VII
           EX Burst	no
           Multiplayable	no
           Limit Break	yes
           Abilities
           (Cards with {LB} cannot be included in your main deck.)
           Limit Break ― 1
           When Zack enters the field, choose 1 Forward. Deal it 3000 damage.
     */
        yield return Card.Code("22-112R")
            .Named("Zack")
            .Cost(3, "R")
            .Category("VII")
            .Job("SOLDIER")
            .Forward()
            .Power(7000)
            .LimitBreak(1)
            .Text(
                "Limit Break ― 1\nWhen Zack enters the field, choose 1 Forward. Deal it 3000 damage."
            )
            .TriggeredAbility(p =>
            {
                p.Text = "When Zack enters the field, choose 1 Forward. Deal it 3000 damage.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () => new DealDamageToTargets(3000);
                p.TargetSelector.AddEffect(t => t.Is.Forward().On.Battlefield());
            });
    }
}
