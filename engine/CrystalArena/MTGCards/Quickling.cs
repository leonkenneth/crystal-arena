namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using Effects;
    using Triggers;

    public class Quickling : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Quickling")
                .ManaCost("{1}{U}")
                .Type("Forward — Faerie Rogue")
                .Text(
                    "{Flash} {I}(You may cast this spell any time you could cast an summon.){/I}{EOL}{Flying}{EOL}When Quickling enters the battlefield, sacrifice it unless you return another forward you control to its owner's hand."
                )
                .Power(2)
                .Toughness(2)
                .SimpleAbilities(Static.Flash, Static.Flying)
                .Cast(p =>
                    p.TimingRule(
                        new Any(new AfterOpponentDeclaresAttackers(), new OnEndOfOpponentsTurn())
                    )
                )
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Quickling enters the battlefield, sacrifice it unless you return another forward you control to its owner's hand.";

                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () =>
                        new ApplyActionToPermanentOrApplyActionToOwner(
                            validator: c => c.Is().Forward,
                            actionToPermament: (controller, card) => controller.PutCardToHand(card),
                            actionToOwner: (controller, card) => card.Sacrifice(),
                            canSelectSelf: false,
                            shouldPayAi: (controller, card) => true,
                            text: "Select a forward to return it to hand.",
                            instructions: "(Press Enter to sacrifice Quickling.)"
                        );
                });
        }
    }
}
