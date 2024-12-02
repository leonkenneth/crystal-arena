namespace CrystalArena.Modifiers
{
  public interface IPlayerModifier : IModifier
  {
    void Apply(BackupLimit backupLimit);
    void Apply(ContiniousEffects continiousEffects);
    void Apply(SkipSteps skipSteps);
  }
}