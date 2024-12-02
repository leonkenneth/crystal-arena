namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class ManaLeech : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mana Leech")
        .ManaCost("{2}{B}")
        .Type("Forward Leech")
        .Text(
          "You may choose not to untap Mana Leech during your untap step.{EOL}{T}: Tap target backup. It doesn't untap during its controller's untap step for as long as Mana Leech remains tapped.")
        .Power(1)
        .Toughness(1)
        .MayChooseToUntap()
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}: Tap target backup. It doesn't untap during its controller's untap step for as long as Mana Leech remains tapped.";
            p.Cost = new Tap();

            p.Effect = () => new CompoundEffect(
              new TapTargets(),
              new ApplyModifiersToTargets(() =>
                {
                  var modifier = new AddSimpleAbility(Static.DoesNotUntap);
                  modifier.AddLifetime(new ModifierSourceGetsUntapedLifetime());
                  return modifier;
                }));

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Backup).On.Battlefield());            
            p.TimingRule(new OnOpponentsTurn(Step.Upkeep));
            p.TargetingRule(new EffectTapBackup());
          });
    }
  }
}