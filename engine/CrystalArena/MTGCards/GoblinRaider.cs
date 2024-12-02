namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class GoblinRaider : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Raider")
        .ManaCost("{1}{R}")
        .Type("Forward Goblin Warrior")
        .Text("Goblin Raider can't block.")
        .FlavorText("He was proud to wear the lizard skin around his waist, just for the fun of annoying the enemy.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.CannotBlock);
    }
  }
}