namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;

  public class VeiledApparition : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Veiled Apparition")
        .ManaCost("{1}{U}")
        .Type("Monster")
        .Text(
          "When an opponent casts a spell, if Veiled Apparition is an monster, Veiled Apparition becomes a 3/3 Illusion forward with flying and 'At the beginning of your upkeep, sacrifice Veiled Apparition unless you pay {1}{U}.'")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a spell, if Veiled Apparition is an monster, Veiled Apparition becomes a 3/3 Illusion forward with flying and 'At the beginning of your upkeep, sacrifice Veiled Apparition unless you pay {1}{U}.'";
            p.Trigger(new OnCastedSpell((c, ctx) =>
              ctx.Opponent == c.Controller && ctx.OwningCard.Is().Monster));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToForward(
                power: 3,
                toughness: 3,
                type: t => t.Change(baseTypes: "forward", subTypes: "illusion"),
                colors: L(CardColor.Water)),
              () => new AddSimpleAbility(Static.Flying),
              () =>
                {
                  var tp = new TriggeredAbility.Parameters();
                  tp.Text = "At the beginning of your upkeep, sacrifice Veiled Apparition unless you pay {1}{U}.";
                  tp.Trigger(new OnStepStart(Step.Upkeep));
                  tp.Effect =
                    () => new PayManaThen("{1}{U}".Parse(),
                      effect: new SacrificeOwner(),
                      parameters: new PayThen.Parameters()
                      {
                        ExecuteIfPaid = false,
                        Message = "Pay upkeep? (or sacrifice Veiled Apparition)",
                      });

                  return new AddTriggeredAbility(new TriggeredAbility(tp));
                });

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}