namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.Effects;
    using CrystalArena.Events;
    using CrystalArena.Triggers;

    public class TreefolkMystic : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Treefolk Mystic")
                .ManaCost("{3}{G}")
                .Type("Forward Treefolk")
                .Text(
                    "Whenever Treefolk Mystic blocks or becomes blocked by a forward, destroy all Auras attached to that forward."
                )
                .FlavorText(
                    "Urza's wards fell from him like autumn leaves as he entered the dreaming CrystalArena. He awoke imprisoned in living wood."
                )
                .Power(2)
                .Toughness(4)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever Treefolk Mystic blocks or becomes blocked by a forward, destroy all Auras attached to that forward.";

                    p.Trigger(new WhenThisBlocks());
                    p.Trigger(new WhenThisBecomesBlocked(triggerForEveryBlocker: true));

                    p.Effect = () =>
                        new DestroyAttachedAttachments(
                            P(
                                (
                                    e =>
                                    {
                                        var message = e.TriggerMessage<BlockerJoinedCombatEvent>();

                                        return message.Blocker.Card == e.Source.OwningCard
                                            ? message.Party.Attackers
                                            : new[] { message.Blocker.Card };
                                    }
                                )
                            ),
                            (c, ctx) => c.Is().Aura
                        );
                });
        }
    }
}
