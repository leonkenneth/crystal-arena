namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class Worship : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Worship")
        .ManaCost("{3}{W}")
        .Type("Monster")
        .Text(
          "If you control a forward, damage that would reduce your life total to less than 1 reduces it to 1 instead.")
        .FlavorText("Believe in the ideal, not the idol.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenYouDontControlSamePermanent());
          })
        .StaticAbility(p => p.Modifier(
          () => new AddDamagePrevention(modifier => new PreventLifelossBelowOneToPlayer(modifier.SourceCard.Controller))));
    }
  }
}