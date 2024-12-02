namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;

  public class Rebuild : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rebuild")
        .ManaCost("{2}{U}")
        .Type("Summon")
        .Text("Return all artifacts to their owners' hands.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new ReturnAllPermanentsToHand(c => c.Is().Artifact);

            p.TimingRule(new Any(              
              new AfterOpponentDeclaresAttackers(),
              new OnEndOfOpponentsTurn()));
          });
    }
  }
}