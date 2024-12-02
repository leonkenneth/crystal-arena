namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;

  public class Quash : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Quash")
        .ManaCost("{2}{U}{U}")
        .Type("Summon")
        .Text(
          "Counter target summon or sorcery spell. Search its controller's breakZone, hand, and library for all cards with the same name as that spell and exile them. Then that player shuffles his or her library.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new CounterTargetSpell(),
              new RemoveFromPlayCardsWithSameNameAsTargetFromGhl());
               
            p.TargetSelector.AddEffect(trg => trg
              .Is.CounterableSpell(e => e.Source.OwningCard.Is().Summon || e.Source.OwningCard.Is().Sorcery)
              .On.Stack());

            p.TargetingRule(new EffectCounterspell());
            p.TimingRule(new WhenTopSpellIsCounterable());
          });
    }
  }
}