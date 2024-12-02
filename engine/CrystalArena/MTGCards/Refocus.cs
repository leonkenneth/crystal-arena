namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Refocus : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Refocus")
        .ManaCost("{1}{U}")
        .Type("Summon")
        .Text("Untap target forward.{EOL}Draw a card.")
        .FlavorText("\"Before you can open your third eye, you must prove you can open the first two.\"")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new UntapTargetPermanents(),
            new DrawCards(1));

          p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

          p.TimingRule(new Any(
            new OnFirstMain(),
            new AfterOpponentDeclaresAttackers()));

          p.TargetingRule(new EffectUntapPermanent());
        });
    }
  }
}
