namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class HopeAndGlory : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hope and Glory")
        .ManaCost("{1}{W}")
        .Type("Summon")
        .Text("Untap two target forwards. Each of them gets +1/+1 until end of turn.")
        .FlavorText("Serra ruled by faith. I cannot afford that luxury.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new UntapTargetPermanents(),
              new ApplyModifiersToTargets(() => new AddPowerAndToughness(1, 1) {UntilEot = true}));

            p.TargetSelector.AddEffect(
              trg => trg.Is.Forward().On.Battlefield(),
              trg => {                
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TimingRule(new AfterOpponentDeclaresAttackers());
            p.TargetingRule(new EffectUntapPermanent());
          });
    }
  }
}