namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using System.Linq;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Triggers;

  public class ImpendingDisaster : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Impending Disaster")
        .ManaCost("{1}{R}")
        .Type("Monster")
        .Text(
          "At the beginning of your upkeep, if there are seven or more backups on the battlefield, sacrifice Impending Disaster and destroy all backups.")
        .FlavorText("The goblins are in charge of maintenance? Why not just set it on fire now and call it a day?")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, if there are seven or more backups on the battlefield, sacrifice Impending Disaster and destroy all backups.";

            p.Trigger(new OnStepStart(Step.Upkeep)
              {Condition = ctx => ctx.Players.Permanents().Count(c => c.Is().Backup) >= 7});

            p.Effect = () => new CompoundEffect(
              new SacrificeOwner(),
              new DestroyAllPermanents((c, ctx) => c.Is().Backup));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}