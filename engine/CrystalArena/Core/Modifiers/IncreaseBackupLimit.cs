namespace CrystalArena.Modifiers
{
    public class IncreaseBackupLimit : Modifier, IPlayerModifier
    {
        private readonly IntegerIncrement _integerIncrement;
        private BackupLimit _backupLimit;

        private IncreaseBackupLimit() { }

        public IncreaseBackupLimit(int amount = 1)
        {
            _integerIncrement = new IntegerIncrement(amount);
        }

        public override void Apply(BackupLimit backupLimit)
        {
            _backupLimit = backupLimit;
            _integerIncrement.Initialize(ChangeTracker);
            _backupLimit.AddModifier(_integerIncrement);
        }

        protected override void Unapply()
        {
            _backupLimit.RemoveModifier(_integerIncrement);
        }
    }
}
