namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.Costs;
    using CrystalArena.Modifiers;

    public class CitanulHierophants : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Citanul Hierophants")
                .ManaCost("{3}{G}")
                .Type("Forward Human Druid")
                .Text("Forwards you control have '{T}: Add {G} to your mana pool.'")
                .FlavorText(
                    "From deep in the caves beneath the forest, the hierophants planned the druids' raids against the enemy."
                )
                .Power(3)
                .Toughness(2)
                .ContinuousEffect(p =>
                {
                    p.Selector = (card, ctx) => card.Controller == ctx.You && card.Is().Forward;
                    p.Modifier = () =>
                    {
                        var mp = new ManaAbilityParameters
                        {
                            Text = "{T}:  Add {G} to your mana pool.",
                            Priority = ManaSourcePriorities.Forward,
                        };

                        mp.ManaAmount(Mana.Wind);

                        return new AddActivatedAbility(new ManaAbility(mp));
                    };
                });
        }
    }
}
