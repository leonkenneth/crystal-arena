using System.Collections.Generic;
using CrystalArena.Effects;
using CrystalArena.Triggers;

namespace CrystalArena.FFTCGCards.Opus22;

public class Opus22_123R_Leo : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        /*
       Rarity	Rare
       Set	Opus XXII (Hidden Hope)
       Element
       Water
       Type	Forward
       Cost	3
       Power	3000
       Job	King
       Categories
       FFCC
       EX Burst	no
       Multiplayable	no
       Limit Break	yes
       Abilities
       (Cards with {LB} cannot be included in your main deck.)
       Limit Break ― 1
       When Leo enters the field, draw 1 card.
     */
        yield return Card
            .Code("22-123R")
            .Named("Leo")
            .Cost(3, "U")
            .Category("FFCC")
            .Job("King")
            .Forward()
            .Power(3000)
            .LimitBreak(1)
            .Text("Limit Break ― 1\nWhen Leo enters the field, draw 1 card.")
            .TriggeredAbility(p =>
            {
                p.Text = "When Leo enters the field, draw 1 card.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () => new DrawCards(1);
            });
    }
}