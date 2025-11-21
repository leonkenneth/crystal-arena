namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class BarrageOfBoulders : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Barrage of Boulders")
                .ManaCost("{2}{R}")
                .Type("Sorcery")
                .Text(
                    "Barrage of Boulders deals 1 damage to each forward you don't control.{EOL}{I}Ferocious{/I} — If you control a forward with power 4 or greater, forwards can't block this turn."
                )
                .FlavorText("Crude tactics can be effective nonetheless.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new FerociousEffect(
                            L(
                                new DealDamageToForwardsAndPlayers(
                                    amountForward: 1,
                                    filterForward: (effect, card) =>
                                        card.Controller != effect.Source.OwningCard.Controller
                                )
                            ),
                            L(
                                new ApplyModifiersToPlayer(
                                    selector: e => e.Controller,
                                    modifiers: () =>
                                    {
                                        var pr = new ContinuousEffectParameters
                                        {
                                            Selector = (card, effect) => card.Is().Forward,
                                            Modifier = () =>
                                                new AddSimpleAbility(Static.CannotBlock),
                                        };

                                        return new AddContiniousEffect(new ContinuousEffect(pr))
                                        {
                                            UntilEot = true,
                                        };
                                    }
                                )
                            )
                        );

                    p.TimingRule(new OnMainStepsOfYourTurn());
                });
        }
    }
}
