using CrystalArena.Infrastructure;

namespace CrystalArena.AI
{
  using System.Collections.Generic;
  using System.Linq;
  using CombatRules;

  public class AttackStrategy
  {
    private readonly AttackStrategyParameters _p;

    public AttackStrategy(AttackStrategyParameters p)
    {
      _p = p;
    }

    public List<Card> ChooseAttackers()
    {
      var parties = GeneratePossibleParties(_p.AttackerCandidates);
      var blockers = _p.BlockerCandidates;
      blockers.Add(null!);
      var assignments = parties.SelectMany(party =>
      {
        return blockers.Select(blocker => { return new BlockAssignment(party, blocker, _p.DefendingPlayersLife); });
      }).ToList();


      return assignments.OrderByDescending(x => x.Score).First().Attackers;
    }

    private List<List<Card>> GeneratePossibleParties(List<Card> candidates)
  {
    var head = candidates.First();
    var tail = candidates.Skip(1).ToList();

    if (tail.None()) return [[head]];
    
    var tailParties = GeneratePossibleParties(tail);
    var result = new List<List<Card>>();
    foreach (var party in tailParties)
    {
      result.Add(party);
      var newParty = new List<Card>(party);
      newParty.Add(head);
      result.Add(newParty);
    }

    return result;
     }

    private class BlockAssignment
    {
      public readonly List<Card> Attackers;
      public readonly int Score;

      public BlockAssignment(List<Card> attackers, Card blocker, int defendersLife)
      {
        Attackers = attackers;
        Score = CalculateScore(new EvaluationParameters(attackers, blocker, defendersLife));
      }       

      private static CombatEvaluationParameters GetAttackerParamers(IEnumerable<CardWithCombatAbilities> attackers,
        CardWithCombatAbilities? blocker)
      {
        var p = new CombatEvaluationParameters(
          attackers.Select(a => a.Card),
          blocker?.Card
        );
        return p;
      }


      private int CalculateAttackersScore(EvaluationParameters p)
      {
        if (p.Attackers.Any(a => a.Card.Has().Deathtouch))
          return 0;

        return p.Attackers.Sum(attacker =>
        {
          if (QuickCombat.CanAttackerBeDealtLeathalDamage(attacker.Card, p.Blocker?.Card) && !attacker.Abilities.CanRegenerate)
            return attacker.Card.Score;

          return 0;
        });
      }

      private int CalculateBlockerScore(EvaluationParameters p)
      {
        var blocker = p.Blocker;
        if (blocker == null)
          return 0;

        var blockerParameters = new CombatEvaluationParameters(p.Attackers.Select(x => x.Card), blocker.Card);
        var canBeKilled = QuickCombat.CanBlockerBeDealtLeathalCombatDamage(blockerParameters);

        if (canBeKilled)
          return 0;

        return blocker.Card.Score;
      }

      private int CalculateScore(EvaluationParameters p)
      {
        return CalculateBlockerScore(p)
          + CalculateLifelossScore(p)
          - CalculateAttackersScore(p);
      }

      private int CalculateLifelossScore(EvaluationParameters p)
      {
        if (p.Blocker != null) return 0;
        var attackerParameters = GetAttackerParamers(p.Attackers, null);
        return ScoreCalculator.CalculateLifelossScore(p.DefendersLife,
          QuickCombat.CalculateDefendingPlayerLifeloss(attackerParameters).Total);
      }

      private class CardWithCombatAbilities
      {
        public readonly CombatAbilities Abilities;
        public readonly Card Card;

        public CardWithCombatAbilities(Card card)
        {
          Card = card;
          Abilities = card.GetCombatAbilities();
        }
      }

      private class EvaluationParameters
      {
        public readonly List<CardWithCombatAbilities> Attackers;
        public readonly CardWithCombatAbilities? Blocker;
        public readonly int DefendersLife;

        public EvaluationParameters(IEnumerable<Card> attackers, Card? blocker, int defendersLife)
        {
          DefendersLife = defendersLife;
          Blocker = blocker == null ? null : new CardWithCombatAbilities(blocker);
          Attackers = attackers.Select(x => new CardWithCombatAbilities(x)).ToList();
        }
      }
    }
  }
}
