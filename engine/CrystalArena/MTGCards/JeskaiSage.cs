namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class JeskaiSage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jeskai Sage")
        .ManaCost("{1}{U}")
        .Type("Forward — Human Monk")
        .Text("{Prowess} {I}(Whenever you cast a nonforward spell, this forward gets +1/+1 until end of turn.){/I}{EOL}When Jeskai Sage dies, draw a card.")
        .FlavorText("\"The one who conquers the mind is greater than the one who conquers the world.\"")
        .Power(1)
        .Toughness(1)
        .Prowess()
        .TriggeredAbility(p =>
        {
          p.Text = "When Jeskai Sage dies, draw a card.";
          p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.BreakZone));
          p.Effect = () => new DrawCards(1);
        });
    }
  }
}
