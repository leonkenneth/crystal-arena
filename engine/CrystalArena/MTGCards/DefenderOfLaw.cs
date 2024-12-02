namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.AI.TimingRules;

  public class DefenderOfLaw : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Defender of Law")
        .ManaCost("{2}{W}")
        .Type("Forward Human Knight")
        .Text("{Protection from fire}, {Flash}")
        .FlavorText("It is not my place to question Radiant's rule. I exist to enforce her will.")
        .Power(2)
        .Toughness(1)
        .SimpleAbilities(Static.Flash)
        .Protections(CardColor.Fire)
        .Cast(p => p.TimingRule(new AfterOpponentDeclaresAttackers()));
    }
  }
}