namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class AbsoluteGrace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Absolute Grace")
        .ManaCost("{1}{W}")
        .Type("Monster")
        .Text("All forwards have protection from dark.")
        .FlavorText(
          "In pursuit of Urza, the Phyrexians sent countless foul legions into Serra's realm. Though beaten back, they left it tainted with uncleansable evil.")
        .Cast(p =>
          {
            p.TimingRule(new OnFirstMain());
            p.Effect = () => new CastPermanent().SetTags(EffectTag.Protection);
          })
        .ContinuousEffect(p =>
          {
            p.Selector = (card, ctx) => card.Is().Forward;
            p.Modifier = () => new AddProtectionFromColors(CardColor.Dark);
          });
    }
  }
}