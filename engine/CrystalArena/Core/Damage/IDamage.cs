using CrystalArena.Infrastructure;

namespace CrystalArena;

public interface IDamage
{
    void Initialize(ChangeTracker changeTracker);
    int Amount { get; set; }
    IDamageSource Source { get; }
    bool IsCombat { get; }
    bool CanBePrevented { get; }
    bool IsLeathal { get; }
    IDamageable Target { get; }
    bool WasAlreadyRedirected(DamageRedirection damageRedirection);
    void AddRedirection(DamageRedirection damageRedirection);
}
