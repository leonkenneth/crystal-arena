namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class MarkedByHonor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Marked by Honor")
        .ManaCost("{3}{W}")
        .Type("Monster - Aura")
        .Text("Enchant forward{EOL}Enchanted forward gets +2/+2 and has brave.{I}(Attacking doesn't cause it to tap.){/I}")
        .FlavorText("Stand your post for duty. Stand your ground for honor.")
        .Cast(p =>
        {
          p.Effect = () => new Attach(
            () => new AddSimpleAbility(Static.Brave),
            () => new AddPowerAndToughness(2, 2))
            .SetTags(EffectTag.IncreasePower, EffectTag.IncreasePower);

          p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
          p.TimingRule(new OnFirstMain());
          p.TargetingRule(new EffectCombatMonster());
        });
    }
  }
}
