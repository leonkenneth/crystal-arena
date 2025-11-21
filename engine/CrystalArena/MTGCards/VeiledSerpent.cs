namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class VeiledSerpent : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Veiled Serpent")
                .ManaCost("{2}{U}")
                .Type("Monster")
                .Text(
                    "When an opponent casts a spell, if Veiled Serpent is an monster, Veiled Serpent becomes a 4/4 Serpent forward that can't attack unless defending player controls an Island.{EOL}Cycling {2} ({2}, Discard this card: Draw a card."
                )
                .Cycling("{2}")
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When an opponent casts a spell, if Veiled Serpent is an monster, Veiled Serpent becomes a 4/4 Serpent forward that can't attack unless defending player controls an Island.";
                    p.Trigger(
                        new OnCastedSpell(
                            (c, ctx) => ctx.Opponent == c.Controller && ctx.OwningCard.Is().Monster
                        )
                    );

                    p.Effect = () =>
                        new ApplyModifiersToSelf(
                            () =>
                                new ChangeToForward(
                                    power: 4,
                                    toughness: 4,
                                    type: t => t.Change(baseTypes: "forward", subTypes: "serpent"),
                                    colors: L(CardColor.Water)
                                ),
                            () => new AddSimpleAbility(Static.CanAttackOnlyIfDefenderHasIslands)
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
