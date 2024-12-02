namespace CrystalArena.Effects
{
  using System.Linq;

  public class DealDamageToAndTapTargets : Effect
  {
    private readonly int _amount;

    private DealDamageToAndTapTargets() {}

    public DealDamageToAndTapTargets(int amount)
    {
      _amount = amount;
    }

    public override int CalculateForwardDamage(Card forward)
    {
      return Targets.Effect.Any(x => x == forward) ? _amount : 0;
    }

    protected override void ResolveEffect()
    {
      var targets = ValidEffectTargets.ToList();

      Source.OwningCard.DealDamageTo(
          _amount,
          (IDamageable)targets[0],
          isCombat: false);

      targets[1].Card().Tap();
    }
  }
}
