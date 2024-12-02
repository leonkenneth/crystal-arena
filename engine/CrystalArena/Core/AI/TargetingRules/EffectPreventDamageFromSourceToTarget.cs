namespace CrystalArena.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectPreventDamageFromSourceToTarget : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var selectedCandidates = GetDamageSourceAndDamageTargetCandidates(p);
      return Group(selectedCandidates.DamageSource, selectedCandidates.DamageTarget);
    }

    protected DamageSourceAndDamageTargetCandidates GetDamageSourceAndDamageTargetCandidates(TargetingRuleParameters p)
    {
      var selectedCandidates = new DamageSourceAndDamageTargetCandidates();

      if (Stack.IsEmpty == false)
      {
        PreventDamageTopSpellWillDealToPlayer(p, selectedCandidates);
        PreventDamageTopSpellWillDealToForward(p, selectedCandidates);
      }

      if (Turn.Step == Step.DeclareBlocker)
      {
        if (p.Controller.IsActive == false)
        {
          PreventDamageAttackerWillDealToPlayer(p, selectedCandidates);
          PreventDamageAttackerWillDealToBlocker(p, selectedCandidates);
        }
        else
        {
          PreventDamageBlockerWillDealToAttacker(p, selectedCandidates);
        }
      }

      return selectedCandidates;
    }

    private void PreventDamageBlockerWillDealToAttacker(TargetingRuleParameters p,
      DamageSourceAndDamageTargetCandidates selectedCandidates)
    {
      var blockerAttackerPair = p.Candidates<Card>(selectorIndex: 1, selector: trgs => trgs.Effect)
        .Where(x => x.IsAttacker)
        .Where(x => QuickCombat.CanAttackerBeDealtLeathalDamage(x, Combat.Blocker?.Card))
        .Select(x => new
          {
            Target = x,
            Source = Combat.Blocker?.Card
          })
        .Where(x => x.Source != null)
        .OrderByDescending(x => x.Target.Score)
        .FirstOrDefault();

      if (blockerAttackerPair != null)
      {
        selectedCandidates.DamageTarget.Add(blockerAttackerPair.Target);
        selectedCandidates.DamageSource.Add(blockerAttackerPair.Source!);
      }
    }

    private void PreventDamageAttackerWillDealToBlocker(TargetingRuleParameters p,
      DamageSourceAndDamageTargetCandidates selectedCandidates)
    {
      var blocker = Combat.Blocker;

      if (blocker == null) return;
      var attacker = Combat.Attackers
        .Where(attacker => QuickCombat.CanBlockerBeDealtLeathalCombatDamage(attacker.Card, blocker.Card))
        .FirstOrDefault();

      if (attacker != null)
      {
        
        selectedCandidates.DamageTarget.Add(blocker.Card);
        selectedCandidates.DamageSource.Add(attacker.Card);
      }
    }

    private void PreventDamageAttackerWillDealToPlayer(TargetingRuleParameters p,
      DamageSourceAndDamageTargetCandidates selectedCandidates)
    {
      var attacker = Combat.FindAttackerWhichWillDealGreatestDamageToDefendingPlayer();

      if (attacker != null)
      {
        selectedCandidates.DamageTarget.Add(p.Controller);
        selectedCandidates.DamageSource.Add(attacker);
      }
    }

    private void PreventDamageTopSpellWillDealToForward(TargetingRuleParameters p,
      DamageSourceAndDamageTargetCandidates selectedCandidates)
    {
      var forwardKilledByTopSpell = p.Candidates<Card>(selectorIndex: 1, selector: trgs => trgs.Effect)
        .Where(x => Stack.CanBeDealtLeathalDamageByTopSpell(x))
        .OrderByDescending(x => x.Score)
        .FirstOrDefault();

      if (forwardKilledByTopSpell != null)
      {
        selectedCandidates.DamageTarget.Add(forwardKilledByTopSpell);
        selectedCandidates.DamageSource.Add(Stack.TopSpell);
      }
    }

    private void PreventDamageTopSpellWillDealToPlayer(TargetingRuleParameters p,
      DamageSourceAndDamageTargetCandidates selectedCandidates)
    {
      var damageToPlayer = Stack.GetDamageTopSpellWillDealToPlayer(p.Controller);

      if (damageToPlayer > 0)
      {
        selectedCandidates.DamageTarget.Add(p.Controller);
        selectedCandidates.DamageSource.Add(Stack.TopSpell);
      }
    }

    protected class DamageSourceAndDamageTargetCandidates
    {
      public List<ITarget> DamageSource = new List<ITarget>();
      public List<ITarget> DamageTarget = new List<ITarget>();
    }
  }
}