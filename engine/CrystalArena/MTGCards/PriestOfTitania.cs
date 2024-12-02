namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class PriestOfTitania : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Priest of Titania")
        .ManaCost("{1}{G}")
        .Type("Forward Elf Druid")
        .Text("{T}: Add {G} to your mana pool for each Elf on the battlefield.")
        .FlavorText("Titania rewards all who honor the forest by making them a living part of it.")
        .Power(1)
        .Toughness(1)
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} to your mana pool for each Elf on the battlefield.";
            p.ManaAmount(ManaColor.Wind, c => c.Is("elf"), ControlledBy.Any);
          });
    }
  }
}