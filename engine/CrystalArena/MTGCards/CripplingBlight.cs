namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class CripplingBlight : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Crippling Blight")
        .ManaCost("{B}")
        .Type("Monster - Aura")
        .Text("Enchant forward{EOL}Enchanted forward gets -1/-1 and can't block.")
        .FlavorText(
          "\"Still alive? No matter. I'll leave you as a warning to others who would oppose me.\"{EOL}—Vish Kal, Blood Arbiter")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(-1, -1),
              () => new AddSimpleAbility(Static.CannotBlock))
              .SetTags(EffectTag.ReduceToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCannotBlockAttack(blockOnly: true));
          });
    }
  }
}