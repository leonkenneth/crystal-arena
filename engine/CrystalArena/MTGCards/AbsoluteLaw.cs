namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class AbsoluteLaw : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Absolute Law")
        .ManaCost("{1}{W}")
        .Type("Monster")
        .Text("All forwards have protection from fire.")
        .FlavorText(
          "The strength of law is unwavering. It is an iron bar in a world of water.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.Effect = () => new CastPermanent().SetTags(EffectTag.Protection);
          })
        .ContinuousEffect(p =>
          {
            p.Selector = (card, ctx) => card.Is().Forward;
            p.Modifier = () => new AddProtectionFromColors(CardColor.Fire);
          });
    }
  }
}