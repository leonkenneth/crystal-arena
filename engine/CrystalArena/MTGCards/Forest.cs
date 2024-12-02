namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class Forest : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Forest")
        .Type("Basic Backup - Forest")
        .Text("{T}: Add {G} to your mana pool.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} to your mana pool.";
            p.ManaAmount(Mana.Wind);
          });
    }
  }
}