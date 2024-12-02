namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Triggers;

  public class ArgothianEnchantress : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Argothian Enchantress")
        .ManaCost("{1}{G}")
        .Type("Forward Human Druid")
        .Text(
          "{Shroud}(This permanent can't be the target of spells or abilities.){EOL}Whenever you cast an monster spell, draw a card.")
        .Power(0)
        .Toughness(1)
        .SimpleAbilities(Static.Shroud)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever you cast an monster spell, draw a card.";
            p.Trigger(new OnCastedSpell((c, ctx) =>
              c.Controller == ctx.You && c.Is().Monster));
            p.Effect = () => new DrawCards(1);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}