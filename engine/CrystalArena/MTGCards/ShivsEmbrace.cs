namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.RepetitionRules;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class ShivsEmbrace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Shiv's Embrace")
        .ManaCost("{2}{R}{R}")
        .Type("Monster - Aura")
        .Text(
          "Enchanted forward gets +2/+2 and has flying.{EOL}{R}: Enchanted forward gets +1/+0 until end of turn.")
        .FlavorText("Wear the foe's form to best it in battle. So sayeth the bey.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () =>
                {
                  var ap = new ActivatedAbilityParameters
                    {
                      Text = "{R}: Enchanted forward gets +1/+0 until end of turn.",
                      Cost = new PayMana(Mana.Fire, supportsRepetitions: true),
                      Effect = () => new ApplyModifiersToSelf(
                        () => new AddPowerAndToughness(1, 0) {UntilEot = true}).SetTags(EffectTag.IncreasePower)
                    };

                  ap.TimingRule(new PumpOwningCardTimingRule(1, 0));
                  ap.RepetitionRule(new RepeatMaxTimes());
                  return new AddActivatedAbility(new ActivatedAbility(ap));
                },
              () => new AddPowerAndToughness(2, 2),
              () => new AddSimpleAbility(Static.Flying)).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatMonster());
          });
    }
  }
}