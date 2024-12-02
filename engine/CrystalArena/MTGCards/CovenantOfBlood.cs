namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class CovenantOfBlood : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Covenant of Blood")
        .ManaCost("{6}{B}")
        .Type("Sorcery")
        .Text("{Convoke} {I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}Covenant of Blood deals 4 damage to target forward or player and you gain 4 life.")
        .SimpleAbilities(Static.Convoke)
        .Cast(p =>
        {
          p.Text = "Covenant of Blood deals 4 damage to target forward or player and you gain 4 life.";
          p.Effect = () => new DealDamageToTargets(amount: 4, gainLife: true);

          p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());

          p.TargetingRule(new EffectDealDamage(4));
        });
    }
  }
}
