namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.RepetitionRules;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.Modifiers;

  public class GhituWarCry : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ghitu War Cry")
        .ManaCost("{2}{R}")
        .Type("Monster")
        .Text("{R}: Target forward gets +1/+0 until end of turn.")
        .FlavorText("The war cry is not simply a shout but a sacrament.")
        .ActivatedAbility(p =>
          {
            p.Text = "{R}: Target forward gets +1/+0 until end of turn.";
            p.Cost = new PayMana(Mana.Fire, supportsRepetitions: true);
            p.Effect = () => new ApplyModifiersToTargets(() => new AddPowerAndToughness(
              1, 0) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

            p.TimingRule(new PumpTargetCardTimingRule());
            p.TargetingRule(new EffectPumpSummon(1, 0));
            p.RepetitionRule(new RepeatMaxTimes());
          });
    }
  }
}