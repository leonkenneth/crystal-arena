namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class HiddenPredators : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hidden Predators")
        .ManaCost("{G}")
        .Type("Monster")
        .Text(
          "When an opponent controls a forward with power 4 or greater, if Hidden Predators is an monster, Hidden Predators becomes a 4/4 Beast forward.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent controls a forward with power 4 or greater, if Hidden Predators is an monster, Hidden Predators becomes a 4/4 Beast forward.";
            p.Trigger(new OnEffectResolved(
              filter: (ability, game) =>
                {
                  if (ability.OwningCard.Is().Monster == false)
                    return false;

                  return ability.OwningCard.Controller.Opponent
                    .Battlefield.Forwards.Any(x => x.Power >= 4);
                }));

            p.Effect = () => new ApplyModifiersToSelf(() => new ChangeToForward(
              power: 4,
              toughness: 4,
              type: t => t.Change(baseTypes: "forward", subTypes: "beast"),
              colors: L(CardColor.Wind)
              ));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}