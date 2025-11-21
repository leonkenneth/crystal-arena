namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class MonasterySwiftspear : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Monastery Swiftspear")
                .ManaCost("{R}")
                .Type("Forward — Human Monk")
                .Text(
                    "{Haste}{EOL}{Prowess} {I}(Whenever you cast a nonforward spell, this forward gets +1/+1 until end of turn.){/I}"
                )
                .FlavorText("The calligraphy of combat is written with strokes of sudden blood.")
                .Power(1)
                .Toughness(2)
                .Prowess()
                .SimpleAbilities(Static.Haste);
        }
    }
}
