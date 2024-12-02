namespace CrystalArena
{
  using Infrastructure;

  public class PreventDamageDealtToAndByForward : DamagePrevention
  {
    private readonly Card _forward;
    private readonly bool _combatOnly;

    private PreventDamageDealtToAndByForward() {}

    public PreventDamageDealtToAndByForward(Card forward, bool combatOnly)
    {
      _forward = forward;
      _combatOnly = combatOnly;
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        base.CalculateHash(calc),
        calc.Calculate(_forward));
    }

    public override int PreventDamage(PreventDamageParameters p)
    {
      if (_combatOnly && !p.IsCombat)
        return 0;

      if (p.Source == Modifier.SourceCard)
        return p.Amount;

      if (p.Target == Modifier.SourceCard)
        return p.Amount;

      return 0;
    }
  }
}