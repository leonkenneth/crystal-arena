namespace CrystalArena
{
  using Modifiers;

  public class BackupLimit : Characteristic<int?>, IAcceptsPlayerModifier
  {
    private BackupLimit() {}

    public BackupLimit(int value)
      : base(value) {}

    public void Accept(IPlayerModifier modifier)
    {
      modifier.Apply(this);
    }
  }
}