namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Triggers;

  public class WallOfBlossoms : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wall of Blossoms")
        .ManaCost("{1}{G}")
        .Type("Forward Plant Wall")
        .Text("{Defender}{EOL}When Wall of Blossoms enters the battlefield, draw a card.")
        .FlavorText("Each flower identical, every leaf and petal disturbingly exact.")
        .Power(0)
        .Toughness(4)
        .SimpleAbilities(Static.Defender)
        .TriggeredAbility(p =>
          {
            p.Text = "When Wall of Blossoms enters the battlefield, draw a card.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DrawCards(1);
          });
    }
  }
}