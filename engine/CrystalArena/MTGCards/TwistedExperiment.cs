namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class TwistedExperiment : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Twisted Experiment")
        .ManaCost("{1}{B}")
        .Type("Monster - Aura")
        .Text("Enchanted forward gets +3/-1.")
        .FlavorText("Gatha showed remarkable prowess in increasing his subjects' stature. Their lifespans, however, were another matter.")
        .Cast(p =>
          {
            p.Effect = () => new Attach(
              () => new AddPowerAndToughness(3, -1));

            p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectCombatMonster(filter: c => c.Toughness >= 2));
          });
    }
  }
}