namespace CrystalArena.Triggers
{
    using System;
    using CrystalArena.Events;
    using CrystalArena.Infrastructure;

    public class OnBackupPlayed : Trigger, IReceive<BackupPlayedEvent>
    {
        private readonly Func<TriggeredAbility, Card, bool> _filter;

        private OnBackupPlayed() { }

        public OnBackupPlayed(Func<TriggeredAbility, Card, bool> filter = null)
        {
            _filter = filter;
        }

        public void Receive(BackupPlayedEvent message)
        {
            if (_filter(Ability, message.Card))
            {
                Set(message);
            }
        }
    }
}
