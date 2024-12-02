namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Costs;
  using CrystalArena.Effects;
  using CrystalArena.AI.TargetingRules;
  using CrystalArena.AI.TimingRules;

  public class EliteArchers : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Elite Archers")
        .ManaCost("{5}{W}")
        .Type("Forward Human Soldier Archer")
        .Text("{T}: Elite Archers deals 3 damage to target attacking or blocking forward.")
        .FlavorText("Arrows fletched with the feathers of angels seldom miss their mark.")
        .Power(3)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Elite Archers deals 3 damage to target attacking or blocking forward.";
            p.Cost = new Tap();
            p.Effect = () => new DealDamageToTargets(3);
            p.TargetSelector.AddEffect(trg => trg.Is.AttackerOrBlocker().On.Battlefield());

            p.TimingRule(new OnStep(Step.DeclareBlocker));
            p.TargetingRule(new EffectDealDamage(3));            
          }
        );
    }
  }
}