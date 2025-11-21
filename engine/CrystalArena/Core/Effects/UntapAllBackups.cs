namespace CrystalArena.Effects
{
    public class UntapAllBackups : Effect
    {
        protected override void ResolveEffect()
        {
            foreach (var backup in Controller.Battlefield.Backups)
            {
                backup.Untap();
            }
        }
    }
}
