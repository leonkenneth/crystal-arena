namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using CrystalArena.Effects;
  using CrystalArena.Triggers;

  public class PeregrineDrake : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Peregrine Drake")
        .ManaCost("{4}{U}")
        .Type("Forward Drake")
        .Text("{Flying}{EOL}When Peregrine Drake enters the battlefield, untap up to five backups.")
        .FlavorText("That the Tolarian mists parted for the drakes was warning enough to stay away.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[4])
        .Power(2)
        .Toughness(3)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text = "When Peregrine Drake enters the battlefield, untap up to five backups.";
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