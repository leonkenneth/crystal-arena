using System;
using Newtonsoft.Json;

namespace CrystalArena.UserInterface.DamageOrder
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    private readonly Decisions.DamageAssignment _assignment;
    private readonly List<DamageAssignment> _damageAssignments;
    private readonly Blocker _blocker;
    private readonly List<Attacker> _attackers;
    private readonly int _damagesToAssign;

    public override object ToJson()
    {
      return new
      {
        Type = "DamageAssignment",
        CanAccept,
        DamagesToAssign = _damagesToAssign,
        Cards = _attackers.Select(x => Ui.Dialogs.Card.Create(x.Card).ToJson()),
        Assignments = DamageAssignmentsDict(),
        BlockerId = _blocker.Card.Id,
        Oid = base.ToJsonWithOid()
      };
    }

    private Dictionary<int, int> DamageAssignmentsDict()
    {
      var result = new Dictionary<int, int>();
      foreach (var assignment in _damageAssignments)
      {
        result.Add(assignment.Attacker.Card.Id, assignment.Damage ?? 0);
      }

      return result;
    }

    public ViewModel(Blocker blocker, IEnumerable<Attacker> attackers, Decisions.DamageAssignment assignment)
    {
      _blocker = blocker;
      _attackers = attackers.ToList();

      _damagesToAssign =
        blocker.Card.CalculateCombatDamageAmount(toPlayer: false, powerIncrease: blocker.Card.GetCombatAbilities().PowerIncrease);
      _damageAssignments =
        attackers.Select(
          attacker => new DamageAssignment(attacker)).ToList();

      _assignment = assignment;      
    }

    public Card Blocker { get { return _blocker.Card; } }
    public IEnumerable<DamageAssignment> Damages { get { return _damageAssignments; } }

    public bool CanAccept
    {
      get { return _damageAssignments.Sum(da => da.Damage ?? 0) == _damagesToAssign; }
    }
    
    public override void ReceiveMessageType(string type, string jsonMessage)
    {
      if (type == "Accept")
      {
        if (CanAccept) Accept();
        return;
      }

      if (type == "AssignDamage")
      {
        var message = JsonConvert.DeserializeObject<AssignDamageMessage>(jsonMessage);
        AssignDamage(message.CardId, message.Amount);
        return;
      }

      throw new ArgumentException("Unknown message type.");
    }

    class AssignDamageMessage
    {
      public int CardId { get; set; }
      public int Amount { get; set; }
    }
    public void AssignDamage(int cardId, int amount)
    {
      var assignment = _damageAssignments.FirstOrDefault(da => da.Attacker.Card.Id == cardId);
      if (assignment == null) return;

      assignment.Damage = amount;
    }

    public virtual void Accept()
    {
      foreach (var assignment in _damageAssignments)
      {
        if (assignment.Damage == null) continue;
        _assignment.Assign(assignment.Attacker, assignment.Damage.Value);
      }

      Close();
    }

    public virtual void Close() {}

    public interface IFactory
    {
      ViewModel Create(Blocker blocker, IEnumerable<Attacker> attackers, Decisions.DamageAssignment assignment);
    }
  }
}