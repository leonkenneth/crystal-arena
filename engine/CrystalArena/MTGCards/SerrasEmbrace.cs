namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class SerrasEmbrace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Serra's Embrace")
        .ManaCost("{2}{W}{W}")
        .Type("Monster - Aura")
        .Text(
          "Enchant forward{EOL}Enchanted forward gets +2/+2 and has flying and brave.")
        .FlavorText(
          "Lifted beyond herself, for that battle Brindri was an angel of light and fury.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(2, 2),
              () => new AddSimpleAbility(Static.Brave),
              () => new AddSimpleAbility(Static.Flying)).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatMonster());
          });
    }
  }
}