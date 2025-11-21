using System;
using System.Collections.Generic;
using System.Linq;
using CrystalArena.Infrastructure;

namespace CrystalArena;

public class AggregateDamage : IDamage
{
    private readonly AggregateType _aggregateType;
    public IDamageSource Source { get; }
    public bool IsCombat { get; private set; }
    public bool CanBePrevented { get; private set; }
    public bool IsLeathal { get; set; }

    public bool WasAlreadyRedirected(DamageRedirection damageRedirection)
    {
        return false;
    }

    public void AddRedirection(DamageRedirection damageRedirection)
    {
        return;
    }

    public void Initialize(ChangeTracker changeTracker)
    {
        Damages.ToList().ForEach(c => c.Initialize(changeTracker));
    }

    public int Amount
    {
        get
        {
            if (_aggregateType == AggregateType.Max)
            {
                return Damages.Max(c => c.Amount);
            }
            else
            {
                return Damages.Sum(c => c.Amount);
            }
        }
        set { throw new ArgumentException("Cannot set Amount of AggregateDamage."); }
    }
    public IDamageable Target { get; private set; }

    public enum AggregateType
    {
        Max,
        Sum,
    }

    public AggregateDamage(Party source, IEnumerable<Damage> damages, IDamageable target)
    {
        Damages = damages;
        Source = source;
        Target = target;
    }

    public IEnumerable<Damage> Damages { get; set; }
}
