namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Effects;
  using Events;
  using Triggers;

  public class CircleOfFlame : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Circle of Flame")
        .ManaCost("{1}{R}")
        .Type("Monster")
        .Text(
          "Whenever a forward without flying attacks you or a planeswalker you control, Circle of Flame deals 1 damage to that forward.")
        .FlavorText("\"Which do you think is a better deterrent: a moat of water or one of fire?\"—Chandra Nalaar")
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever a forward without flying attacks you or a planeswalker you control, Circle of Flame deals 1 damage to that forward.";

            p.Trigger(new WhenAForwardAttacks(t =>
              t.You && t.AttackerHas(c => !c.Has().Flying)));

            p.Effect = () => new DealDamageToForward(
              amount: 1,
              forward: P(e => e.TriggerMessage<AttackerJoinedCombatEvent>().Attacker.Card));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}