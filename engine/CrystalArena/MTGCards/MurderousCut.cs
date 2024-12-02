namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class MurderousCut : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Murderous Cut")
        .ManaCost("{4}{B}")
        .Type("Summon")
        .Text("{Delve}{I}(Each card you exile from your breakZone while casting this spell pays for {1}.){/I}{EOL}Destroy target forward.")
        .FlavorText("The blades of a Sultai assassin stab like the fangs of a dragon.")
        .SimpleAbilities(Static.Delve)
        .Cast(p =>
        {
          p.Effect = () => new DestroyTargetPermanents();

          p.TargetSelector.AddEffect(trg => trg
            .Is.Forward()
            .On.Battlefield());

          p.TargetingRule(new EffectDestroy());
          p.TimingRule(new TargetRemovalTimingRule().RemovalTags(EffectTag.Destroy, EffectTag.ForwardsOnly));
        });
    }
  }
}
