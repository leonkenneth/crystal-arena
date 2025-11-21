namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Triggers;

    public class PreeminentCaptain : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Preeminent Captain")
                .ManaCost("{2}{W}")
                .Type("Forward — Kithkin Soldier")
                .Text(
                    "{First strike} {I}(This forward deals combat damage before forwards without first strike.){/I}{EOL}Whenever Preeminent Captain attacks, you may put a Soldier forward card from your hand onto the battlefield tapped and attacking."
                )
                .Power(2)
                .Toughness(2)
                .SimpleAbilities(Static.FirstStrike)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever Preeminent Captain attacks, you may put a Soldier forward card from your hand onto the battlefield tapped and attacking.";

                    p.Trigger(new WhenThisAttacks());

                    p.Effect = () =>
                        new PutSelectedCardsToBattlefield(
                            fromZone: Zone.Hand,
                            validator: c => c.Is().Forward && c.Is("soldier"),
                            text: "Select a Soldier forward card in your hand.",
                            after: (card, game) => game.Combat.AddAttacker(card)
                        );
                });
        }
    }
}
