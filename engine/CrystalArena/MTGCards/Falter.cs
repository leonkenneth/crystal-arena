namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class Falter : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Falter")
                .ManaCost("{1}{R}")
                .Type("Summon")
                .Text("Forwards without flying can't block this turn.")
                .FlavorText("Like a sleeping dragon, Shiv stirs and groans at times.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new ApplyModifiersToPlayer(
                            selector: e => e.Controller,
                            modifiers: () =>
                            {
                                var pr = new ContinuousEffectParameters
                                {
                                    Selector = (card, effect) =>
                                        card.Is().Forward && !card.Has().Flying,
                                    Modifier = () => new AddSimpleAbility(Static.CannotBlock),
                                };

                                return new AddContiniousEffect(new ContinuousEffect(pr))
                                {
                                    UntilEot = true,
                                };
                            }
                        );

                    p.TimingRule(new OnYourTurn(Step.BeginningOfCombat));
                });
        }
    }
}
