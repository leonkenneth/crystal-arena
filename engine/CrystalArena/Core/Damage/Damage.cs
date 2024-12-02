using System.Collections.Generic;
using System.Linq;

namespace CrystalArena
{
  using CrystalArena.Infrastructure;

  [Copyable]
  public class Damage : IHashable, IDamage
  {
    private readonly Trackable<int> _amount;
    private readonly TrackableList<DamageRedirection> _redirections = new TrackableList<DamageRedirection>();

    public Damage(int amount, bool isCombat, bool canBePrevented, Card source, IDamageable target)
    {
      _amount = new Trackable<int>(amount);
      IsCombat = isCombat;
      CanBePrevented = canBePrevented;
      Source = source;
      Target = target;
    }

    private Damage() {}

    public int Amount { get { return _amount.Value; } set { _amount.Value = value; } }
    public bool IsCombat { get; private set; }
    public bool CanBePrevented { get; private set; }
    public IDamageSource Source { get; private set; }
    public IDamageable Target { get; private set; }

    public bool IsLeathal
    {
      get { return Source.HasDeathtouch; }
    }

    public int CalculateHash(HashCalculator calc)
    {
      return Amount;
    }

    public void Initialize(ChangeTracker changeTracker)
    {
      _amount.Initialize(changeTracker);
      _redirections.Initialize(changeTracker);
    }

    public bool WasAlreadyRedirected(DamageRedirection damageRedirection)
    {
      return _redirections.Contains(damageRedirection);
    }

    public void AddRedirection(DamageRedirection damageRedirection)
    {
      _redirections.Add(damageRedirection);
    }
  }
}