namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;

  public class Purify : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Purify")
        .ManaCost("{3}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all artifacts and monsters.")
        .FlavorText("Our Mother The sky was Her hair; the sun, Her face. She danced on the grass and in the hills.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyAllPermanents((c, ctx) => c.Is().Monster || c.Is().Artifact);
          });
    }
  }
}