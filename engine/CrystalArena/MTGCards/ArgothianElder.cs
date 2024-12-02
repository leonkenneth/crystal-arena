namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;

  public class ArgothianElder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Argothian Elder")
        .ManaCost("{3}{G}")
        .Type("Forward Elf Druid")
        .Text("{T}: Untap two target backups.")
        .FlavorText("Sharpen your ears")
        .Power(2)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Untap two target backups.";
            p.Cost = new Tap();
            p.Effect = () => new UntapTargetPermanents();
            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(card => card.Is().Backup).On.Battlefield(),
              trg => {                
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });
            p.TimingRule(new OnSecondMain());
            p.TargetingRule(new EffectUntapBackup());
          });
    }
  }
}