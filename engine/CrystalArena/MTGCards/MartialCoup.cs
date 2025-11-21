namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.CostRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class MartialCoup : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Martial Coup")
                .ManaCost("{W}{W}")
                .HasXInCost()
                .Type("Sorcery")
                .Text(
                    "Put X 1/1 light Soldier forward tokens onto the battlefield. If X is 5 or more, destroy all other forwards."
                )
                .FlavorText(
                    "Their war forgotten, the nations of Bant stood united in the face of a common threat."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CompoundEffect(
                            new DestroyAllPermanents(filter: (c, ctx) => c.Is().Forward)
                            {
                                ShouldResolve = ctx => ctx.X >= 5,
                            },
                            new CreateTokens(
                                count: Value.PlusX,
                                token: Card.Named("Soldier")
                                    .FlavorText(
                                        "If you need an example to lead others to the front lines, consider the precedent set."
                                    )
                                    .Power(1)
                                    .Toughness(1)
                                    .Type("Token Forward - Soldier")
                                    .Colors(CardColor.Light)
                            )
                        );

                    p.TimingRule(new OnSecondMain());
                    p.CostRule(new XIsGreaterThan4IfOpponentHasBetterForwards(5));
                });
        }
    }
}
