namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;

  public class Intervene : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Intervene")
        .ManaCost("{U}")
        .Type("Summon")
        .Text("Counter target spell that targets a forward.")
        .FlavorText("At first I simply observed. But I found that without investment in others, life serves no purpose.")
        .Cast(p =>
          {
            p.Effect = () => new CounterTargetSpell();
            
            p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell(e => 
              e.Targets.Effect.Any(x => x.IsCard() && x.Card().Is().Forward)).On.Stack());            

            p.TimingRule(new WhenTopSpellIsCounterable());
            p.TargetingRule(new EffectCounterspell());
          });
    }
  }
}