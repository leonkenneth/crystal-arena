namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class SerrasSanctum : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Serra's Sanctum")
        .Type("Legendary Backup")
        .Text("{T}: Add {W} to your mana pool for each monster you control.")
        .FlavorText("A fragile cocoon of dreaming will.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {W} to your mana pool for each monster you control.";
            p.ManaAmount(ManaColor.Light, c => c.Is().Monster);
          }
        );
    }
  }
}