namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class NoRestForTheWicked : CardTemplateSource
    {
        private static bool WasPutIntoBreakZoneThisTurnFromBattlefield(Card card, Game game)
        {
            return card.Is().Forward
                && game.Turn.Events.HasChangedZone(
                    card,
                    @from: Zone.Battlefield,
                    to: Zone.BreakZone
                );
        }

        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("No Rest for the Wicked")
                .ManaCost("{1}{B}")
                .Type("Monster")
                .Text(
                    "Sacrifice No Rest for the Wicked: Return to your hand all forward cards in your breakZone that were put there from the battlefield this turn."
                )
                .FlavorText("The soul? Here, we have no use for such frivolities.")
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "Sacrifice No Rest for the Wicked: Return to your hand all forward cards in your breakZone that were put there from the battlefield this turn.";
                    p.Cost = new Sacrifice();

                    p.Effect = () =>
                        new ReturnAllCardsInBreakZoneToHand(
                            WasPutIntoBreakZoneThisTurnFromBattlefield
                        );

                    p.TimingRule(
                        new Any(new OnStep(Step.EndOfTurn), new WhenOwningCardWillBeDestroyed())
                    );
                    p.TimingRule(
                        new WhenYourBreakZoneCountIs(
                            minCount: 1,
                            selector: WasPutIntoBreakZoneThisTurnFromBattlefield
                        )
                    );
                });
        }
    }
}
