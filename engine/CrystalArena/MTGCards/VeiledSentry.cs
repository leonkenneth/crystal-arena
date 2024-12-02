namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.Events;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;

  public class VeiledSentry : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Veiled Sentry")
        .ManaCost("{U}")
        .Type("Monster")
        .Text(
          "When an opponent casts a spell, if Veiled Sentry is an monster, Veiled Sentry becomes an Illusion forward with power and toughness each equal to that spell's converted mana cost.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a spell, if Veiled Sentry is an monster, Veiled Sentry becomes an Illusion forward with power and toughness each equal to that spell's converted mana cost.";
            p.Trigger(new OnCastedSpell((c, ctx) =>
              ctx.Opponent == c.Controller && ctx.OwningCard.Is().Monster));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToForward(
                power: self => self.SourceEffect.TriggerMessage<SpellPutOnStackEvent>().Card.ConvertedCost,
                toughness: self => self.SourceEffect.TriggerMessage<SpellPutOnStackEvent>().Card.ConvertedCost,
                type: self => "Forward Illusion",
                colors: L(CardColor.Water)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}