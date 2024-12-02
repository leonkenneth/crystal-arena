namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;

  public class BurstOfEnergy : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Burst of Energy")
        .ManaCost("{W}")
        .Type("Summon")
        .Text("Untap target permanent")
        .FlavorText("I stand ready to die for our world. Who will stand with me?")
        .Cast(p =>
          {
            p.Effect = () => new UntapTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Card().On.Battlefield());

            p.TimingRule(new Any(
              new OnFirstMain(),
              new AfterOpponentDeclaresAttackers()));

            p.TargetingRule(new EffectUntapPermanent());
          });
    }
  }
}