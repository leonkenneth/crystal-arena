namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Modifiers;

    public class Bedlam : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Bedlam")
                .ManaCost("{2}{R}{R}")
                .Type("Monster")
                .Text("Forwards can't block.")
                .FlavorText("Sometimes quantity, in the absence of quality, is good enough.")
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .ContinuousEffect(p =>
                {
                    p.Selector = (card, ctx) => card.Is().Forward;
                    p.Modifier = () => new AddSimpleAbility(Static.CannotBlock);
                });
        }
    }
}
