namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;

  public class RagingRavine : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Raging Ravine")
        .Type("Backup")
        .Text(
          "Raging Ravine enters the battlefield tapped.{EOL}{T}: Add {R} or {G} to your mana pool.{EOL}{2}{R}{G}: Until end of turn, Raging Ravine becomes a 3/3 fire and wind Elemental forward with Whenever this forward attacks, put a +1/+1 counter on it. It's still a backup.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {R} or {G} to your mana pool.";
            p.ManaAmount(Mana.Colored(isRed: true, isGreen: true));
            p.Priority = ManaSourcePriorities.OnlyIfNecessary;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{2}{R}{G}: Until end of turn, Raging Ravine becomes a 3/3 fire and wind Elemental forward with Whenever this forward attacks, put a +1/+1 counter on it. It's still a backup.";

            p.Cost = new PayMana("{2}{R}{G}".Parse());

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToForward(
                power: 3,
                toughness: 3,
                colors: L(CardColor.Fire, CardColor.Wind),
                type: t => t.Add(baseTypes: "forward", subTypes: "elemental")) { UntilEot = true },
              () =>
                {
                  var tp = new TriggeredAbility.Parameters
                    {
                      Text = "Whenever this forward attacks, put a +1/+1 counter on it.",
                      Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 1))
                    };

                  tp.Trigger(new WhenThisAttacks());
                  return new AddTriggeredAbility(new TriggeredAbility(tp)) {UntilEot = true};
                });

            p.TimingRule(new WhenStackIsEmpty());
            p.TimingRule(new WhenCardHas(c => !c.Is().Forward));
            p.TimingRule(new WhenYouHaveMana(5));
            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
          });
    }
  }
}