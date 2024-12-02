namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class Treachery : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Treachery")
        .ManaCost("{3}{U}{U}")
        .Type("Monster Aura")
        .Text("When Treachery enters the battlefield, untap up to five backups.{EOL}You control enchanted forward.")
        .FlavorText("The academy educates; I employ. It's a perfect arrangement.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() => new ChangeController(m => m.SourceCard.Controller))
              .SetTags(EffectTag.ChangeController);

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectGainControl());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Treachery enters the battlefield, untap up to five backups.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new UntapSelectedPermanents(
              minCount: 0,
              maxCount: 5,
              validator: c => c.Is().Backup,
              text: "Select backups to untap."
              );
          }
        );
    }
  }
}