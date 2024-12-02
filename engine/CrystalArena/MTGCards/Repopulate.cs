namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Repopulate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Repopulate")
        .ManaCost("{1}{G}")
        .Type("Summon")
        .Text(
          "Shuffle all forward cards from target player's breakZone into that player's library.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new ShuffleTargetBreakZoneIntoMainDeck(c => c.Is().Forward);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TargetingRule(new EffectAnyPlayer());
            p.TimingRule(new OnEndOfOpponentsTurn());
          });
    }
  }
}