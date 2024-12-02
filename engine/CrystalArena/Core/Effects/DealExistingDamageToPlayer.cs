namespace CrystalArena.Effects
{
  public class DealExistingDamageToPlayer : Effect
  {
    private readonly DynParam<IDamage> _damage;
    private readonly DynParam<Player> _player;

    private DealExistingDamageToPlayer() {}

    public DealExistingDamageToPlayer(DynParam<IDamage> damage, DynParam<Player> player)
    {
      _damage = damage;
      _player = player;

      RegisterDynamicParameters(damage, _player);
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return player == _player.Value ? _damage.Value.Amount : 0;
    }

    public override int CalculateForwardDamage(Card forward)
    {
      return 0;
    }

    protected override void ResolveEffect()
    {
      _player.Value.ReceiveDamage(_damage.Value);
    }
  }
}