namespace CrystalArena.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectTapForward : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Where(x => p.Controller.IsActive ? x.CanBlock() : x.CanAttack)
        .Select(x => new
          {
            Card = x,
            Damage = CalculateAttackerScoreForThisTurn(x)
          })
        .Where(x => x.Damage > 0)
        .OrderByDescending(x => x.Damage)
        .ThenByDescending(x => x.Card.Score)
        .Select(x => x.Card);

      return Group(candidates, p.TotalMinTargetCount(), p.TotalMaxTargetCount());
    }
  }
}