namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class RankAndFile : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Rank and File")
                .ManaCost("{2}{B}{B}")
                .Type("Forward Zombie")
                .Text(
                    "When Rank and File enters the battlefield, wind forwards get -1/-1 until end of turn."
                )
                .FlavorText(
                    "Left, right, left, right . . . hmm. Okay—left, left, left, left, . . ."
                )
                .Power(3)
                .Toughness(3)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Rank and File enters the battlefield, wind forwards get -1/-1 until end of turn.";
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () =>
                        new ApplyModifiersToPermanents(
                            selector: (c, ctx) => c.Is().Forward && c.HasColor(CardColor.Wind),
                            modifier: () => new AddPowerAndToughness(-1, -1) { UntilEot = true }
                        )
                        {
                            ToughnessReduction = 1,
                        };
                });
        }
    }
}
