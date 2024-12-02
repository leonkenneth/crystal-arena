namespace CrystalArena.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectPreventCombatDamage : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      if (IsAfterOpponentDeclaresAttackers(p.Controller))
      {
        var attackerCandidates = p.Candidates<Card>(ControlledBy.Opponent)
          .Where(x => x.IsAttacker)
          .OrderByDescending(CalculateAttackerScoreForThisTurn);

        return Group(attackerCandidates, p.TotalMinTargetCount(), p.TotalMaxTargetCount());
      }

      if (IsAfterOpponentDeclaresBlocker(p.Controller) && Combat.Blocker != null)
      {
        var candidates = new List<Card>(){ Combat.Blocker!.Card };
        return Group(candidates, p.TotalMinTargetCount(), p.TotalMaxTargetCount());
      }

      return None<Targets>();
    }
  }
}