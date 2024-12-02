namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Triggers;

  public class WurmcoilEngine : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wurmcoil Engine")
        .ManaCost("{6}")
        .Type("Artifact Forward - Wurm")
        .Text(
          "{Deathtouch}, {Lifelink}{EOL}When Wurmcoil Engine dies, put a 3/3 colorless Wurm artifact forward token with deathtouch and a 3/3 colorless Wurm artifact forward token with lifelink onto the battlefield.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Deathtouch, Static.Lifelink)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Wurmcoil Engine dies, put a 3/3 colorless Wurm artifact forward token with deathtouch and a 3/3 colorless Wurm artifact forward token with lifelink onto the battlefield.";

            p.Trigger(new OnZoneChanged(
              @from: Zone.Battlefield,
              to: Zone.BreakZone));

            p.Effect = () => new CreateTokens(
              Card
                .Named("Wurm")
                .Text("{Deathtouch}")
                .FlavorText("When wurms aren't hungry.{EOL}—Nantuko expression meaning 'never'")
                .Power(3)
                .Toughness(3)
                .Type("Token Artifact Forward - Wurm")
                .Colors(CardColor.Colorless)
                .SimpleAbilities(Static.Deathtouch),
              Card
                .Named("Wurm")
                .Text("{Lifelink}")
                .FlavorText("When wurms aren't hungry.{EOL}—Nantuko expression meaning 'never'")
                .Power(3)
                .Toughness(3)
                .Type("Token Artifact Forward - Wurm")
                .Colors(CardColor.Colorless)
                .SimpleAbilities(Static.Lifelink));
          });
    }
  }
}