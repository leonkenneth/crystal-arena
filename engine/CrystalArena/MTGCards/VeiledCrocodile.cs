namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class VeiledCrocodile : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Veiled Crocodile")
                .ManaCost("{2}{U}")
                .Type("Monster")
                .Text(
                    "When a player has no cards in hand, if Veiled Crocodile is an monster, Veiled Crocodile becomes a 4/4 Crocodile forward."
                )
                .FlavorText("Some roads are paved with bad intentions.")
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When a player has no cards in hand, if Veiled Crocodile is an monster, Veiled Crocodile becomes a 4/4 Crocodile forward.";
                    p.Trigger(
                        new OnEffectResolved(
                            filter: (ability, game) =>
                            {
                                if (ability.OwningCard.Is().Monster == false)
                                    return false;

                                return game.Players.Any(x => x.Hand.Count == 0);
                            }
                        )
                    );

                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new ChangeToForward(
                                power: 4,
                                toughness: 4,
                                type: t => t.Change(baseTypes: "forward", subTypes: "crocodile"),
                                colors: L(CardColor.Water)
                            )
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
