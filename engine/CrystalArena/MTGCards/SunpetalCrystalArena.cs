namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Infrastructure;

  public class SunpetalCrystalArena : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sunpetal CrystalArena")
        .Type("Backup")
        .Text(
          "Sunpetal CrystalArena enters the battlefield tapped unless you control a Forest or a Plains.{EOL}{T}: Add {G} or {W} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(
          tap: P(e => e.Controller.Battlefield.None(card => card.Is("forest") || card.Is("plains")))))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} or {W} to your mana pool.";
            p.ManaAmount(Mana.Colored(isWhite: true, isGreen: true));
          });
    }
  }
}