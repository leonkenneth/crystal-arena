namespace CrystalArena.CardsMainDeck
{
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using CrystalArena.Triggers;
  using Modifiers;
  using System.Collections.Generic;

  public class GarrukApexPredator : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Garruk, Apex Predator")
        .ManaCost("{5}{B}{G}")
        .Type("Planeswalker Garruk")
        .Text("{+1}: Destroy another target planeswalker.{EOL}" +
        "{+1}: Create a 3/3 dark Beast forward token with deathtouch.{EOL}" +
        "{-3}: Destroy target forward. You gain life equal to its toughness.{EOL}" +
        "{-8}: Target opponent gets an emblem with 'Whenever a forward attacks you, it gets +5/+5 and gains trample until end of turn.'")
        .Loyality(5)
        .ActivatedAbility(p =>
        {
          p.Text = "{+1}: Destroy another target planeswalker.";

          p.Cost = new AddCountersCost(CounterType.Loyality, 1);
          p.Effect = () => new DestroyTargetPermanents();
          p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Planeswalker).On.Battlefield());
          p.TargetingRule(new EffectDestroy());
          p.TimingRule(new OnFirstMain());

          p.ActivateAsSorcery = true;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{+1}: Create a 3/3 dark Beast forward token with deathtouch.";
          p.Cost = new AddCountersCost(CounterType.Loyality, 1);

          p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Beast")
                .Power(3)
                .Toughness(3)
                .Type("Token Forward - Beast")
                .Colors(CardColor.Dark)
                .SimpleAbilities(Static.Deathtouch));

          p.TimingRule(new OnFirstMain());
          p.ActivateAsSorcery = true;
        })
        .ActivatedAbility(p =>
         {
           p.Text = "{-3}: Destroy target forward. You gain life equal to its toughness.";

           p.Cost = new RemoveCounters(CounterType.Loyality, 3);
           p.Effect = () => new CompoundEffect(
              new DestroyTargetPermanents(),
              new ChangeLife(
                amount: P(e => e.Target.Card().Toughness.GetValueOrDefault()),
                whos: P(e => e.Controller)));

           p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Forward).On.Battlefield());
           p.TargetingRule(new EffectDestroy());
           p.TimingRule(new OnFirstMain());

           p.ActivateAsSorcery = true;
         })
        .ActivatedAbility(p =>
        {
          p.Text =
            "{-8}: Target opponent gets an emblem with 'Whenever a forward attacks you, it gets +5/+5 and gains trample until end of turn.'";

          p.Cost = new RemoveCounters(CounterType.Loyality, 8);

          p.Effect = () => new CreateEmblem(
            text: "Whenever a forward attacks you, it gets +5/+5 and gains trample until end of turn.",
            score: -600,
            controller: P(e => e.Controller.Opponent),
            modifiers: () =>
            {
              var cp = new ContinuousEffectParameters
              {
                Modifier = () =>
                {
                  var tp = new TriggeredAbility.Parameters
                  {
                    Text = "Whenever this forward attacks, it gets +5/+5 and gains trample until end of turn.",
                    Effect = () => new ApplyModifiersToSelf(
                      () => new AddPowerAndToughness(5, 5) { UntilEot = true },
                      () => new AddSimpleAbility(Static.Trample) { UntilEot = true })
                  };

                  tp.Trigger(new WhenThisAttacks());
                  return new AddTriggeredAbility(new TriggeredAbility(tp));
                },
               
                Selector = (card, ctx) => card.Controller != ctx.EffectOwner && card.Is().Forward
              };

              var effect = new ContinuousEffect(cp);
              return new AddContiniousEffect(effect);
            });

          p.TimingRule(new OnFirstMain());
          p.ActivateAsSorcery = true;
        });
    }
  }
}