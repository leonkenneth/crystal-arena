namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Modifiers;

  public class RadiantArchangel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Radiant, Archangel")
        .ManaCost("{3}{W}{W}")
        .Type("Legendary Forward Angel")
        .Text(
          "{Flying}, {brave}{EOL}Radiant, Archangel gets +1/+1 for each other forward with flying on the battlefield.")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(Static.Flying, Static.Brave)
        .StaticAbility(p =>
          {
            p.Modifier(() => new ModifyPowerToughnessForEachPermanent(
              power: 1,
              toughness: 1,
              filter: (c, ctx) => c.Is().Forward && c.Has().Flying && c != ctx.OwningCard,
              modifier: () => new IntegerIncrement(),
              controlledBy: ControlledBy.Any
              ));

            p.EnabledInAllZones = false;
          });
    }
  }
}