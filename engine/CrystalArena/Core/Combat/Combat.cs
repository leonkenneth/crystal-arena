using CrystalArena.AI;

namespace CrystalArena
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Events;
  using Infrastructure;

  [Copyable]
  public class Combat : GameObject, IHashable
  {
    private readonly TrackableList<Attacker> _attackers = new TrackableList<Attacker>();
    private Trackable<Blocker?> _blocker = new Trackable<Blocker?>();
    private readonly TrackableList<AssignedDamage> _assignedDamage = new TrackableList<AssignedDamage>();
    private DamageAssignment _blockerDamageAssignment;

    public IEnumerable<Attacker> Attackers { get { return _attackers; } }
    public Blocker? Blocker => _blocker.Value;
    public int AttackerCount { get { return _attackers.Count; } }
    public int BlockersCount { get { return _blocker.Value == null ? 0 : 1; } }

    private Player DefendingPlayer { get { return Players.Defending; } }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(_attackers),
        calc.Calculate(_blocker.Value));
    }

    public void Initialize(Game game)
    {
      Game = game;

      _attackers.Initialize(ChangeTracker);
      _blocker.Initialize(ChangeTracker);
      _assignedDamage.Initialize(ChangeTracker);
    }

    public void DistributeCombatDamage(bool firstStrike = false)
    {
      if (firstStrike)
      {
        DistributeFirstStrikeCombatDamage();
      }
      else
      {
        DistributeNormalStrikeCombatDamage();
      }
    }

    private void DistributeDamageToDefendingPlayer(Party party)
    {
      var target = (IDamageable) Players.Defending;
      _assignedDamage.Add(new AssignedDamage(
        amount: QuickCombat.CalculateDefendingPlayerLifeloss(party, null),
        source: party,
        target: target));
    }

    public void DistributeFirstStrikeCombatDamage()
    {
      var party = new Party(Attackers);
      
      if (Blocker == null && !party.HasFirstStrike)
      {
        return;
      }

      if (Blocker == null && party.HasFirstStrike)
      {
        DistributeDamageToDefendingPlayer(party);
        return;
      }

      var blocker = Blocker!;
      if (blocker.HasFirstStrike)
      {
        foreach (var assignment in _blockerDamageAssignment.AttackerDamageAssignments)
        {
          if (party.Contains(assignment.Key))
          {
            _assignedDamage.Add(new AssignedDamage(
              assignment.Value,
              blocker.Card,
              assignment.Key.Card));
          }
        }
      }

      if (party.HasFirstStrike)
      {
        var damageToBlocker =
          QuickCombat.GetAmountOfDamagePartyWillDealToForward2(party,
            forward2: blocker.Card);
        _assignedDamage.Add(new AssignedDamage(
          amount: damageToBlocker,
          source: party,
          target: blocker.Card
        ));
      }
    }
    
    public void DistributeNormalStrikeCombatDamage()
    {
      var party = new Party(Attackers);
      
      if (Blocker == null && !party.HasFirstStrike) // party has normal strike, damage will be dealt at this point
      {
        DistributeDamageToDefendingPlayer(party);
        return;
      }

      if (Blocker == null && party.HasFirstStrike)
      {
        return;
      }

      var blocker = Blocker!;
      if (!blocker.HasFirstStrike)
      {
        foreach (var assignment in _blockerDamageAssignment.AttackerDamageAssignments)
        {
          if (party.Contains(assignment.Key))
          {
            _assignedDamage.Add(new AssignedDamage(
              assignment.Value,
              blocker.Card,
              assignment.Key.Card));
          }
        }
      }

      if (!party.HasFirstStrike)
      {
        var damageToBlocker =
          QuickCombat.GetAmountOfDamagePartyWillDealToForward2(party,
            forward2: blocker.Card);
        _assignedDamage.Add(new AssignedDamage(
          amount: damageToBlocker,
          source: party,
          target: blocker.Card
        ));
      }
    }

    public bool CanAttackersBeBlockedByAny(IEnumerable<Card> forwards)
    {
      foreach (var attacker in _attackers)
      {
        foreach (var forward in forwards)
        {
          if (attacker.CanBeBlockedBy(forward))
            return true;
        }
      }

      return false;
    }    

    public void DealAssignedDamage()
    {     
      foreach (var assignedDamage in _assignedDamage)
      {
        IDamage damage;
        if (assignedDamage.SourceIsSingleCard)
        {
          damage = new Damage(
            amount: assignedDamage.Amount.Total,
            source: assignedDamage.Source.SingleCard,
            isCombat: true,
            canBePrevented: true,
            target: assignedDamage.Target);
        }
        else
        {
          var damages = new List<Damage>();
          foreach(var assignedDamageContribution in assignedDamage.Amount.Contributions)
          {
            damages.Add(new Damage(
              amount: assignedDamageContribution.Value,
              source: assignedDamageContribution.Key,
              isCombat: true,
              canBePrevented: true,
              target: assignedDamage.Target));
          }

          damage = new AggregateDamage(
            source: assignedDamage.Source,
            damages: damages,
            target: assignedDamage.Target);

        }
        

        damage.Initialize(ChangeTracker);
        assignedDamage.Target.ReceiveDamage(damage);
      }
      
      _assignedDamage.Clear();
    }

    public void AddAttacker(Card card)
    {
      var attacker = CreateAttacker(card);
      card.AttackCount += 1;
      _attackers.Add(attacker);

      if (!card.Has().Brave)
        card.Tap();

      Publish(new AttackerJoinedCombatEvent(attacker));
    }

    public void SetBlocker(Card card)
    {
      var blocker = new Blocker(card, Game);
      _blocker.Value = blocker;

      Publish(new BlockerJoinedCombatEvent(blocker, Attackers));
    }

    public bool IsAttacker(Card card)
    {
      return FindAttacker(card) != null;
    }    

    public bool IsBlocker(Card card)
    {
      return Blocker?.Card == card;
    }

    public void Remove(Card card)
    {
      var attacker = FindAttacker(card);

      if (attacker != null)
      {
        _attackers.Remove(attacker);
        attacker.RemoveFromCombat();
        return;
      }

      if (Blocker != null)
      {
        Blocker.RemoveFromCombat();
        _blocker.Value = null;
      }
    }

    public void RemoveAll()
    {
      Blocker?.RemoveFromCombat();

      foreach (var attacker in _attackers)
      {
        attacker.RemoveFromCombat();
      }

      _blocker.Value = null;
      _attackers.Clear();
    }

    public void SetDamageAssignments()
    {
      Enqueue(new SetDamageAssignments(
        Players.Defending,
        _attackers,
        _blocker));
    }

    public bool AnyForwardsWithFirstStrike()
    {
      return _attackers.Any(x => x.Card.HasFirstStrike) ||
        (Blocker?.Card.HasFirstStrike ?? false);
    }

    public bool AnyForwardsWithNormalStrike()
    {
      return _attackers.Any(x => x.Card.HasNormalStrike) ||
             (Blocker?.Card.HasNormalStrike ?? false);
    }

    public bool CanBeDealtLeathalCombatDamage(Card card)
    {
      if (card == Blocker?.Card)
      {
        return Blocker.WillBeDealtLeathalCombatDamage();
      }

      if (Blocker == null) return false;
      var attacker = FindAttacker(card);
      if (attacker != null)
      {
        return QuickCombat.CanAttackerBeDealtLeathalDamage(attacker, Blocker.Card);
      }

      return false;
    }

    public bool CanBlockAtLeastOneAttacker(Card card)
    {
      return Attackers.Any(attacker => attacker.CanBeBlockedBy(card));
    }

    public int CountHowManyThisCouldBlock(Card card)
    {
      var opponent = card.Controller.Opponent;
      return opponent.Battlefield.ForwardsThatCanAttack.Count(x => x.CanBeBlockedBy(card));
    }

    public bool CouldBeBlockedByAny(Card card)
    {
      var opponent = card.Controller.Opponent;
      return opponent.Battlefield.ForwardsThatCanBlock.Any(card.CanBeBlockedBy);
    }

    public bool WillAnyAttackerDealDamageToDefender()
    {
      return FindAttackerWhichWillDealGreatestDamageToDefendingPlayer() != null;
    }

    public Card FindAttackerWhichWillDealGreatestDamageToDefendingPlayer(Func<Card, bool> filter = null)
    {
      filter = filter ?? delegate { return true; };

      return Attackers
        .Where(x => filter(x))
        .Select(x => new
          {
            Attacker = x,
            Damage = QuickCombat.CalculateDefendingPlayerLifeloss(x.Card, null)
          })
        .Where(x => x.Damage > 0)
        .OrderByDescending(x => x.Damage)
        .Select(x => x.Attacker)
        .FirstOrDefault();
    }

    public bool CanKillAny(Card attackerOrBlocker)
    {
      if (attackerOrBlocker == Blocker?.Card)
      {
        return Blocker.CanKillAnyAttacker();
      }
      
      var attacker = FindAttacker(attackerOrBlocker);
      if (attacker != null)
      {
        return attacker.CanKillBlocker();
      }

      return false;
    }

    private Attacker CreateAttacker(Card card)
    {
      return new Attacker(card, Game);
    }

    private Blocker CreateBlocker(Card blocker)
    {
      return new Blocker(blocker, Game);
    }

    public Attacker? FindAttacker(Card cardAttacker)
    {
      return _attackers.FirstOrDefault(a => a.Card == cardAttacker);
    }

    [Copyable]
    public class AssignedDamage
    {
      public readonly PartyDamage Amount;
      public readonly Party Source;
      public readonly IDamageable Target;
      public bool SourceIsSingleCard => Source.Attackers.Count() == 1;

      private AssignedDamage() { }

      public AssignedDamage(PartyDamage amount, Party source, IDamageable target)
      {
        Amount = amount;
        Source = source;
        Target = target;
      }

      public AssignedDamage(int amount, Card source, IDamageable target)
      {
        Source = new Party(source);
        Amount = new PartyDamage();
        Amount.Add(source, amount);
        Target = target;
      }
    }

    public void SetDamageAssignment(DamageAssignment result)
    {
      _blockerDamageAssignment = result;
    }

    public bool HasBlocker(Card card)
    {
      return IsAttacker(card) && Blocker != null;
    }
  }
}