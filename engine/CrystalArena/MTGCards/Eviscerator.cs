namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Triggers;

  public class Eviscerator : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Eviscerator")
        .ManaCost("{3}{B}{B}")
        .Type("Forward Horror")
        .Text("{Protection from light}{EOL}When Eviscerator enters the battlefield, you lose 5 life.")
        .FlavorText(
          "It roamed the time bubble like a captured animal. Kerrick knew he must soon find a way to unleash it on Tolaria or destroy it.")
        .Power(5)
        .Toughness(5)
        .Protections(CardColor.Light)
        .TriggeredAbility(p =>
          {
            p.Text = "When Eviscerator enters the battlefield, you lose 5 life.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ChangeLife(amount: -5, whos: P(e => e.Controller));
          });
    }
  }
}