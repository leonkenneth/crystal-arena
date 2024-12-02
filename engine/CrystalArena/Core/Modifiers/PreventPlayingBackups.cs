namespace CrystalArena.Modifiers
{
  public class PreventPlayingBackups : Modifier, IPlayerModifier
  {
    private BackupLimit _backupLimit;
    private readonly IntegerSetter _integerSetter = new IntegerSetter(-1000);

    public override void Apply(BackupLimit backupLimit)
    {
      _backupLimit = backupLimit;
      _integerSetter.Initialize(ChangeTracker);
      _backupLimit.AddModifier(_integerSetter);
    }
    
    protected override void Unapply()
    {
      _backupLimit.RemoveModifier(_integerSetter);
    }
  }
}