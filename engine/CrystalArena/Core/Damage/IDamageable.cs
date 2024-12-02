using System;

namespace CrystalArena
{
  public interface IDamageable
  {
    void ReceiveDamage(IDamage damage);
  }
  
  public static class Damageable
  {
    public static bool IsPermanent(this IDamageable damageable)
    {
      return damageable is Card;
    }

    public static bool IsPlayer(this IDamageable damageable)
    {
      return damageable is Player;
    }

    public static Card Permanent(this IDamageable damageable)
    {
      return damageable as Card;
    }
  }
}

