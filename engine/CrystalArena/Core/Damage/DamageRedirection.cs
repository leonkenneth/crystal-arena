namespace CrystalArena
{
  using CrystalArena.Infrastructure;

  [Copyable]
  public abstract class DamageRedirection : GameObject, IHashable
  {
    public Modifier Modifier { get; private set; }

    public abstract int CalculateHash(HashCalculator calc);    

    public bool RedirectDamage(IDamage damage, ITarget target)
    {
      if (damage.WasAlreadyRedirected(this))
        return false;

      var redirect = WillRedirect(damage, target);

      if (redirect)
      {
        damage.AddRedirection(this);
        Redirect(damage, target);
      }

      return redirect;
    }

    public virtual void Initialize(Modifier modifier, Game game)
    {
      Modifier = modifier;
      Game = game;
    }

    protected abstract void Redirect(IDamage damage, ITarget target);
    protected abstract bool WillRedirect(IDamage damage, ITarget target);
  }
}