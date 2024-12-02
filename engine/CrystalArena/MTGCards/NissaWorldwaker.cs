namespace CrystalArena.CardsMainDeck
{
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using CrystalArena.Modifiers;
  using System.Collections.Generic;

  public class NissaWorldwaker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Nissa, Worldwaker")
        .ManaCost("{3}{G}{G}")
        .Type("Planeswalker Nissa")
        .Text("{+1}: Target backup you control becomes a 4/4 Elemental forward with trample. It's still a backup.{EOL}" +
        "{+1}: Untap up to four target Forests.{EOL}" +
        "{-7}: Search your library for any number of basic backup cards, put them onto the battlefield, then shuffle your library. Those backups become 4/4 Elemental forwards with trample. They're still backups.")
        .Loyality(3)
        .ActivatedAbility(p =>
        {
          p.Text = "{+1}: Target backup you control becomes a 4/4 Elemental forward with trample. It's still a backup.";
          p.Cost = new AddCountersCost(CounterType.Loyality, 1);

          p.Effect = () => new ApplyModifiersToTargets(
           () => new ChangeToForward(
              power: 4,
              toughness: 4,
              colors: L(CardColor.Colorless),
              type: t => t.Add(baseTypes: "forward", subTypes: "elemental")),
               () => new AddSimpleAbility(Static.Trample));

          p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Backup, ControlledBy.SpellOwner).On.Battlefield());
          p.TargetingRule(new EffectOrCostRankBy(c => c.Is().Forward || c.IsTapped ? 0 : -c.Score));

          p.TimingRule(new OnFirstMain());
          p.ActivateAsSorcery = true;
        })
        .ActivatedAbility(p =>
          {
            p.Text = "{+1}: Untap up to four target Forests.";
            p.Cost = new AddCountersCost(CounterType.Loyality, 1);

            p.Effect = () => new UntapTargetPermanents();

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is("forest")).On.Battlefield(),
               trg =>
               {
                 trg.MinCount = 0;
                 trg.MaxCount = 4;
               });

            p.TargetingRule(new EffectUntapBackup());

            p.TimingRule(new OnFirstMain());
            p.ActivateAsSorcery = true;
          })
        .ActivatedAbility(p =>
        {
          p.Text = "{-7}: Search your library for any number of basic backup cards, put them onto the battlefield, then shuffle your library. Those backups become 4/4 Elemental forwards with trample. They're still backups.";
          p.Cost = new RemoveCounters(CounterType.Loyality, 7);

          p.Effect = () => new SearchMainDeckPutToZone(
            zone: Zone.Battlefield,
            minCount: 0,
            maxCount: 100,
            validator: (c, ctx) => c.Is().BasicBackup,
            text: "Search your library for any number of basic backup cards.",
            afterPutToZone: (c, ctx) =>
            {
              var mp = new ModifierParameters
              {
                SourceEffect = ctx.Effect,
                SourceCard = ctx.Effect.Source.OwningCard
              };

              var mod1 = new ChangeToForward(
                power: 4,
                toughness: 4,
                colors: L(CardColor.Colorless),
                type: t => t.Add(baseTypes: "forward", subTypes: "elemental"));

              var mod2 = new AddSimpleAbility(Static.Trample);

              c.AddModifier(mod1, mp);
              c.AddModifier(mod2, mp);
            });

          p.TimingRule(new OnFirstMain());
          p.ActivateAsSorcery = true;
        });
    }
  }
}