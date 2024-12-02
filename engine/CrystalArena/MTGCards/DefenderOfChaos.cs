namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.AI.TimingRules;

  public class DefenderOfChaos : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Defender of Chaos")
        .ManaCost("{2}{R}")
        .Type("Forward Human Knight")
        .Text("{Protection from light}, {Flash}")
        .FlavorText("Some knights will not follow orders—only disorder.")
        .Power(2)
        .Toughness(1)
        .SimpleAbilities(Static.Flash)
        .Protections(CardColor.Light)
        .Cast(p => p.TimingRule(new AfterOpponentDeclaresAttackers()));
    }
  }
}