namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class ShamblingAttendants : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Shambling Attendants")
          .ManaCost("{7}{B}")
          .Type("Forward — Zombie")
          .Text("{Delve}{I}(Each card you exile from your breakZone while casting this spell pays for {1}.){/I}{EOL}{Deathtouch}{I}(Any amount of damage this deals to a forward is enough to destroy it.){/I}")
          .FlavorText("\"Let the world behold what becomes of those who defy us.\"{EOL}—Taigam, Sidisi's Hand")
          .Power(3)
          .Toughness(5)
          .SimpleAbilities(Static.Deathtouch, Static.Delve);
    }
  }
}
