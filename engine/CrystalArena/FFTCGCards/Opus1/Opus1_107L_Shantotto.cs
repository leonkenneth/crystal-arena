using System.Collections.Generic;
using CrystalArena.Effects;
using CrystalArena.Modifiers;
using CrystalArena.Triggers;

namespace CrystalArena.FFTCGCards.Opus1;

public class Opus1_107L_Shantotto : CardTemplateSource
{
    public override IEnumerable<CardTemplate> GetCards()
    {
        yield return Card.Code("1-107L")
            .Named("Shantotto")
            .Cost(7, "Y")
            .Categories("DFF", "XI")
            .Job("Mage")
            .Backup()
            .Text(
                "If Shantotto is on the field, it gains Elements of Fire, Ice, Wind, Earth, Lightning, and Water.\nWhen Shantotto enters the field, remove all Forwards from the game."
            )
            .StaticAbility(p =>
            {
                p.Modifiers.Add(() =>
                    new SetColors(
                        CardColor.Fire,
                        CardColor.Ice,
                        CardColor.Wind,
                        CardColor.Earth,
                        CardColor.Lightning,
                        CardColor.Water
                    )
                );
            })
            .TriggeredAbility(p =>
            {
                p.Text = "When Shantotto enters the field, remove all Forwards from the game.";
                p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                p.Effect = () =>
                    new RemoveFromPlayAllCards(
                        Zone.Battlefield,
                        (effect, card) => card.Is().Forward
                    );
            });
    }
}
