namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class GangOfElk : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Gang of Elk")
                .ManaCost("{5}{G}")
                .Type("Forward Elk Beast")
                .Text(
                    "Whenever Gang of Elk becomes blocked, it gets +2/+2 until end of turn for each forward blocking it."
                )
                .FlavorText("The elk is Gaea's favorite, who wears the forest on its brow.")
                .Power(5)
                .Toughness(4)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever Gang of Elk becomes blocked, it gets +2/+2 until end of turn for each forward blocking it.";
                    p.Trigger(new WhenThisBecomesBlocked(triggerForEveryBlocker: true));
                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new AddPowerAndToughness(2, 2) { UntilEot = true }
                        );
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
