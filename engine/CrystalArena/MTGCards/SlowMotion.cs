namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;

  public class SlowMotion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Slow Motion")
        .ManaCost("{2}{U}")
        .Type("Monster Aura")
        .Text(
          "At the beginning of the upkeep of enchanted forward's controller, that player sacrifices that forward unless he or she pays {2}.{EOL}When Slow Motion is put into a breakZone from the battlefield, return Slow Motion to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() =>
              {
                var tp = new TriggeredAbility.Parameters
                  {
                    Text =
                      "At the beginning of the upkeep of enchanted forward's controller, that player sacrifices that forward unless he or she pays {2}.",
                    Effect =
                      () => new PayManaThen(2.Colorless(),
                        effect: new SacrificeOwner(),
                        parameters: new PayThen.Parameters()
                        {
                          ExecuteIfPaid = false,
                          Message = "Pay upkeep cost?",
                        }),
                  };

                tp.Trigger(new OnStepStart(Step.Upkeep));

                return new AddTriggeredAbility(new TriggeredAbility(tp));
              });

            p.TimingRule(new OnSecondMain());
            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.Score, ControlledBy.Opponent));
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Slow Motion is put into a breakZone from the battlefield, return Slow Motion to its owner's hand.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}