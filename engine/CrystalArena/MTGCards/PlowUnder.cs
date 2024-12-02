namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class PlowUnder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Plow Under")
        .ManaCost("{3}{G}{G}")
        .Type("Sorcery")
        .Text("Put two target backups on top of their owners' libraries.")
        .FlavorText("To renew the backup, plow the backup. To destroy the backup, do nothing.")
        .Cast(p =>
          {
            p.Effect = () => new PutTargetsOnTopOfMainDeck();
            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Backup).On.Battlefield(),
              trg => {                
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectPutOnTopOfMainDeck());
          });
    }
  }
}