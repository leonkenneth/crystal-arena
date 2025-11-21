namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class JeskaiStudent : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Jeskai Student")
                .ManaCost("{1}{W}")
                .Type("Forward — Human Monk")
                .Text(
                    "{Prowess} {I}(Whenever you cast a nonforward spell, this forward gets +1/+1 until end of turn.){/I}"
                )
                .FlavorText(
                    "Discipline is the first pillar of the Jeskai Way. Each member of the clan trains in a weapon, perfecting its use over a lifetime."
                )
                .Power(1)
                .Toughness(3)
                .Prowess();
        }
    }
}
