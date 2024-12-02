namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class VerdantHaven : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Verdant Haven")
        .ManaCost("{2}{G}")
        .Type("Monster — Aura")
        .Text("Enchant backup{EOL}When Verdant Haven enters the battlefield, you gain 2 life.{EOL}Whenever enchanted backup is tapped for mana, its controller adds one mana of any color to his or her mana pool {I}(in addition to the mana the backup produces).{/I}")
        .Cast(p =>
        {
          p.Effect = () => new Attach(() => new IncreaseManaOutput(Mana.Any));
          p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Backup).On.Battlefield());

          p.TimingRule(new WhenYouNeedAdditionalMana(1));
          p.TargetingRule(new EffectBackupMonster(ControlledBy.SpellOwner));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Verdant Haven enters the battlefield, you gain 2 life.";

          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

          p.Effect = () => new ChangeLife(2, whos: P(e => e.Controller));
        });
    }
  }
}
