using System.Linq;
using CrystalArena.Infrastructure;

namespace CrystalArena.AI
{
  public class BlockerEvaluation
  {
    private readonly CombatEvaluationParameters _p;

    public BlockerEvaluation(CombatEvaluationParameters p)
    {
      _p = p;
    }

    private int GetBlockerLifepoints()
    {
      return _p.Blocker.Card.Life + _p.Blocker.ToughnessIncrease;
    }

    private int GetTotalAttackerLifepoints()
    {
      return _p.Attackers.Sum(x => x.Card.Life + x.ToughnessIncrease);
    }

    public Results Evaluate()
    {
      var results = new Results
        {
          DamageDealt = 0,
          ReceivesLeathalDamage = false
        };

      if (_p.Attackers.None())
      {
        return results;
      }

      var blockingCard = _p.Blocker.Card;

      if (!blockingCard.Is().Forward || !blockingCard.CanBeDestroyed)
        return results;

      var party = new Party(_p.Attackers);
      if (blockingCard.HasFirstStrike && !party.HasFirstStrike && !party.HasIndestructible)
      {
        var blockerDealtAmount = blockingCard.CalculateCombatDamageAmount(toPlayer: false);

        if (blockerDealtAmount > 0 && blockingCard.Has().Deathtouch)
        {
          return results;
        }

        if (blockerDealtAmount >= GetTotalAttackerLifepoints())
          return results;
      }

      var attackerDealtAmount = QuickCombat.GetAmountOfDamagePartyWillDealToForward2(
        party,
        forward2: _p.Blocker.Card).Total;

      if (attackerDealtAmount == 0)
        return results;


      if (party.HasDeathtouch)
      {
        results.ReceivesLeathalDamage = true;
      }

      results.DamageDealt = attackerDealtAmount;
      results.ReceivesLeathalDamage = results.ReceivesLeathalDamage || attackerDealtAmount >= GetBlockerLifepoints();

      return results;
    }

    public class Results
    {
      public int DamageDealt;
      public bool ReceivesLeathalDamage;
    }
  }
}