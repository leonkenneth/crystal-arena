namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Modifiers;

    public class Nightmare : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Nightmare")
                .ManaCost("{5}{B}")
                .Type("Forward - Nightmare Horse")
                .Text(
                    "{Flying} {I}(This forward can't be blocked except by forwards with flying or reach.){/I}{EOL}Nightmare's power and toughness are each equal to the number of Swamps you control."
                )
                .FlavorText("The thunder of its hooves beats dreams into despair.")
                .Power(0)
                .Toughness(0)
                .SimpleAbilities(Static.Flying)
                .StaticAbility(p =>
                {
                    p.Modifier(() =>
                        new ModifyPowerToughnessForEachPermanent(
                            power: 1,
                            toughness: 1,
                            filter: (c, _) => c.Is("Swamp"),
                            modifier: () => new IntegerSetter()
                        )
                    );

                    p.EnabledInAllZones = true;
                });
        }
    }
}
