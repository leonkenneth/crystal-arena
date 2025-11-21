namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Modifiers;

    public class Festergloom : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Festergloom")
                .ManaCost("{2}{B}")
                .Type("Sorcery")
                .Text("Nonblack forwards get -1/-1 until end of turn.")
                .FlavorText("The death of a scout can be as informative as a safe return.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new ApplyModifiersToPermanents(
                            selector: (c, ctx) => c.Is().Forward && !(c.HasColor(CardColor.Dark)),
                            modifier: () => new AddPowerAndToughness(-1, -1) { UntilEot = true }
                        );
                });
        }
    }
}
