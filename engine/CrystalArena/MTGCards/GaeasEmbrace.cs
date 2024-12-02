namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class GaeasEmbrace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Gaea's Embrace")
        .ManaCost("{2}{G}{G}")
        .Type("Monster - Aura")
        .Text(
          "Enchanted forward gets +3/+3 and has trample.{EOL}{G}: Regenerate enchanted forward.")
        .FlavorText("The forest rose to the battle, not to save the people but to save itself.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () =>
                {
                  var ap = new ActivatedAbilityParameters
                    {
                      Text = "{G}: Regenerate enchanted forward.",
                      Cost = new PayMana(Mana.Wind),
                      Effect = () => new RegenerateOwner()
                    };

                  ap.TimingRule(new RegenerateSelfTimingRule());

                  return new AddActivatedAbility(new ActivatedAbility(ap));
                },
              () => new AddPowerAndToughness(3, 3),
              () => new AddSimpleAbility(Static.Trample)
              ).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatMonster());
          });
    }
  }
}