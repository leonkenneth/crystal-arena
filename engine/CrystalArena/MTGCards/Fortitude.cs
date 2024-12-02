namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;
  using ReturnToHand = Effects.ReturnToHand;

  public class Fortitude : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Fortitude")
        .ManaCost("{1}{G}")
        .Type("Monster Aura")
        .Text(
          "Sacrifice a Forest: Regenerate enchanted forward.{EOL}When Fortitude is put into a breakZone from the battlefield, return Fortitude to its owner's hand.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(() =>
              {
                var ap = new ActivatedAbilityParameters
                  {
                    Text = "Sacrifice a Forest: Regenerate enchanted forward.",
                    Cost = new Sacrifice(),
                    Effect = () => new RegenerateOwner()
                  };

                ap.TargetSelector.AddCost(trg => trg
                  .Is.Card(x => x.Is("forest"), ControlledBy.SpellOwner)
                  .On.Battlefield());

                ap.TimingRule(new RegenerateSelfTimingRule());
                ap.TargetingRule(new CostSacrificeToRegenerate());

                return new AddActivatedAbility(new ActivatedAbility(ap));
              });

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatMonster());
          })
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Fortitude is put into a breakZone from the battlefield, return Fortitude to its owner's hand.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
            p.Effect = () => new ReturnToHand(returnOwningCard: true);
          });
    }
  }
}