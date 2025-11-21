namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class ChargingRhino : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Charging Rhino")
                .ManaCost("{3}{G}{G}")
                .Type("Forward - Rhino")
                .Text("Charging Rhino can't be blocked by more than one forward.")
                .FlavorText(
                    "The knights of Troinir Keep took the rhino as their emblem due to its intense focus when charging down a perceived threat—a trait they discovered on a hunting excursion."
                )
                .Power(4)
                .Toughness(4)
                .SimpleAbilities(Static.CannotBeBlockedByMoreThanOne);
        }
    }
}
