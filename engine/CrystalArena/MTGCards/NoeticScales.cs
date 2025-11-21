namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class NoeticScales : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Noetic Scales")
                .ManaCost("{4}")
                .Type("Artifact")
                .Text(
                    "At the beginning of each player's upkeep, return to its owner's hand each forward that player controls with power greater than the number of cards in his or her hand."
                )
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of each player's upkeep, return to its owner's hand each forward that player controls with power greater than the number of cards in his or her hand.";

                    p.Trigger(
                        new OnStepStart(step: Step.Upkeep, passiveTurn: true, activeTurn: true)
                    );

                    p.Effect = () =>
                        new ReturnAllPermanentsToHand(
                            (e, c) =>
                                c.Is().Forward
                                && c.Controller.IsActive
                                && c.Power > c.Controller.Hand.Count
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
