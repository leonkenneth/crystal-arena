namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class AlpineGrizzly : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Alpine Grizzly")
                .ManaCost("{2}{G}")
                .Type("Forward - Bear")
                .FlavorText(
                    "The Temur welcome bears into the clan, fighting alongside them in battle. The relationship dates back to when they labored side by side under Sultai rule."
                )
                .Power(4)
                .Toughness(2);
        }
    }
}
