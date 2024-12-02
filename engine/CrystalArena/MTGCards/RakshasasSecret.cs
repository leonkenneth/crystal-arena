namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Effects;

  public class RakshasasSecret : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rakshasa's Secret")
        .ManaCost("{2}{B}")
        .Type("Sorcery")
        .Text("Target opponent discards two cards. Put the top two cards of your library into your breakZone.")
        .FlavorText("\"It saddens me to lose a source of inspiration. This one seemed especially promising.\"—Ashiok")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new OpponentDiscardsCards(selectedCount: 2),
            new PlayerPutsTopCardsFromMainDeckToBreakZone(P(e => e.Controller), count: 2));
        });
    }
  }
}
