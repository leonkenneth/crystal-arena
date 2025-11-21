namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class GoblinCadets : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Goblin Cadets")
                .ManaCost("{R}")
                .Type("Forward Goblin")
                .Text(
                    "Whenever Goblin Cadets blocks or becomes blocked, target opponent gains control of it. (This removes Goblin Cadets from combat.)"
                )
                .FlavorText(
                    "'If you kids don't stop that racket, I'm turning this expedition around right now!'"
                )
                .Power(2)
                .Toughness(1)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever Goblin Cadets blocks or becomes blocked, target opponent gains control of it. (This removes Goblin Cadets from combat.)";

                    p.Trigger(new WhenThisBecomesBlocked(triggerForEveryBlocker: false));
                    p.Trigger(new WhenThisBlocks());

                    p.Effect = () => new SwitchController();
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
