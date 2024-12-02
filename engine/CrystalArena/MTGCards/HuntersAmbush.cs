namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class HuntersAmbush : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hunter's Ambush")
        .ManaCost("{2}{G}")
        .Type("Summon")
        .Text("Prevent all combat damage that would be dealt by nongreen forwards this turn.")
        .FlavorText("First you lose your enemy's trail. Then you lose all sense of direction. Then you hear the growls . . .")
        .Cast(p =>
        {
          p.Effect = () => new PreventAllCombatDamage(filter: card => !card.HasColor(CardColor.Wind));
          p.TimingRule(new AfterOpponentDeclaresAttackers());
        });
    }
  }
}
