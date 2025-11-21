namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Modifiers;

    public class SeraphOfTheMasses : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Seraph of the Masses")
                .ManaCost("{5}{W}{W}")
                .Type("Forward — Angel")
                .Text(
                    "{Convoke}{I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}{Flying}{EOL}Seraph of the Masses's power and toughness are each equal to the number of forwards you control."
                )
                .Power(0)
                .Toughness(0)
                .SimpleAbilities(Static.Flying, Static.Convoke)
                .StaticAbility(p =>
                {
                    p.Modifier(() =>
                        new ModifyPowerToughnessForEachPermanent(
                            power: 1,
                            toughness: 1,
                            filter: (c, _) => c.Is().Forward,
                            modifier: () => new IntegerSetter()
                        )
                    );
                    p.EnabledInAllZones = true;
                });
        }
    }
}
