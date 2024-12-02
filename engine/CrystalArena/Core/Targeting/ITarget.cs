namespace CrystalArena
{
  using CrystalArena.Decisions;
  using CrystalArena.Infrastructure;

  public interface ITarget : IHashable
  {
    int Id { get; }
  }

  public static class Target
  {
    public static Zone? Zone(this ITarget target)
    {
      if (target.IsPlayer())
      {
        return null;
      }

      if (target.IsEffect())
      {
        return target.Effect().IsOnStack
          ? CrystalArena.Zone.Stack
          : CrystalArena.Zone.None;
      }

      return target.Card().Zone;
    }

    public static Card Card(this ITarget target)
    {
      return target as Card;
    }

    public static Player Player(this ITarget target)
    {
      return target as Player;
    }

    public static Effect Effect(this ITarget target)
    {
      var effect = target as Effect;

      if (effect != null)
        return effect;

      var lazyEffect = target as ScenarioEffect;

      return lazyEffect != null ? lazyEffect.Effect() : null;
    }

    public static object TargetTypeAndId(this ITarget target)
    {
      if (target.IsPlayer())
      {
        var player = target.Player();
        return new
        {
          TargetType = "Player",
          PlayerId = player.Id
        };
      }

      if (target.IsCard())
      {
        var card = target.Card();
        return new
        {
          TargetType = "Card",
          CardId = card.Id
        };
      }

      return new { };
    }

    public static bool IsCard(this ITarget target)
    {
      return target is Card;
    }

    public static bool IsPlayer(this ITarget target)
    {
      return target is Player;
    }

    public static bool IsEffect(this ITarget target)
    {
      return target is Effect || target is ScenarioEffect;
    }

    public static void ReceiveDamage(this ITarget target, IDamage damage)
    {
      var damageable = target as IDamageable;

      if (damageable != null)
      {
        damageable.ReceiveDamage(damage);
      }
    }

    public static int Life(this ITarget target)
    {
      var haslife = target as IHasLife;

      if (haslife != null)
        return haslife.Life;

      return 0;
    }

    public static Player Controller(this ITarget target)
    {
      if (target.IsPlayer())
        return target.Player();

      if (target.IsEffect())
        return target.Effect().Controller;

      return target.Card().Controller;
    }

    public static ITargetType Is(this ITarget target)
    {
      if (target.IsCard())
      {
        return target.Card().Is();
      }

      return new NotCardNorPartyTargetType();
    }

    private class NotCardNorPartyTargetType : ITargetType
    {
      public bool Artifact { get { return false; } }

      public bool Attachment { get { return false; } }
      public bool BasicBackup { get { return false; } }
      public bool Forward { get { return false; } }
      public bool Monster { get { return false; } }
      public bool Equipment { get { return false; } }
      public bool Summon { get { return false; } }
      public bool Backup { get { return false; } }
      public bool Legendary { get { return false; } }
      public bool Sorcery { get { return false; } }
      public bool Token { get { return false; } }
      public bool Aura { get { return false; } }
      public bool NonBasicBackup { get { return false; } }
      public bool Planeswalker { get { return false; } }
      public bool Party => false;
    }
  }
}