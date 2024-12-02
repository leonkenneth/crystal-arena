namespace CrystalArena.UserInterface.DamageOrder
{
  public class DamageAssignment
  {
    public DamageAssignment(Attacker attacker)
    {
      Attacker = attacker;
    }

    public virtual int? Damage { get; set; }
    public Attacker Attacker { get; private set; }
  }
}