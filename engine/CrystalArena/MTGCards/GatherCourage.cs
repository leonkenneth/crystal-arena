namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class GatherCourage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Gather Courage")
        .ManaCost("{G}")
        .Type("Summon")
        .Text(
          "{Convoke}{I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}Target forward gets +2/+2 until end of turn.")
        .FlavorText("\"Even your shadow is too foul to tolerate.\"")
        .SimpleAbilities(Static.Convoke)
        .Cast(p =>
          {
            p.Text = "Target forward gets +2/+2 until end of turn.";

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(2, 2) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

            p.TargetingRule(new EffectPumpSummon(2, 2));
            p.TimingRule(new PumpTargetCardTimingRule());
          });
    }
  }
}