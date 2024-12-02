namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.CostRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class BlackSunsZenith : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dark Sun's Zenith")
        .ManaCost("{B}{B}").HasXInCost()
        .Type("Sorcery")
        .Text("Put X -1/-1 counters on each forward. Shuffle Dark Sun's Zenith into its owner's library.")
        .FlavorText("Under the suns, Mirrodin kneels and begs us for perfection.")
        .Cast(p =>
          {
            p.AfterResolve = (c, _) => c.ShuffleIntoMainDeck();
            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (c, ctx) => c.Is().Forward,
              modifier: () => new AddCounters(() => new PowerToughness(-1, -1), Value.PlusX))
              {ToughnessReduction = Value.PlusX};

            p.TimingRule(new OnFirstMain());
            p.CostRule(new XIsOptimalDamage());
          });
    }
  }
}