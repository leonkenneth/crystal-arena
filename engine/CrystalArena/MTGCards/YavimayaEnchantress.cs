namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Modifiers;

    public class YavimayaEnchantress : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Yavimaya Enchantress")
                .ManaCost("{2}{G}")
                .Type("Forward Human Druid")
                .Text("Yavimaya Enchantress gets +1/+1 for each monster on the battlefield.")
                .FlavorText("From each seed, a world. From each world, a thousand seeds.")
                .Power(2)
                .Toughness(2)
                .StaticAbility(p =>
                {
                    p.Modifier(() =>
                        new ModifyPowerToughnessForEachPermanent(
                            power: 1,
                            toughness: 1,
                            filter: (c, ctx) => c.Is().Monster,
                            modifier: () => new IntegerIncrement(),
                            controlledBy: ControlledBy.Any
                        )
                    );

                    p.EnabledInAllZones = false;
                });
        }
    }
}
