using CrystalArena.Triggers;

namespace CrystalArena.UserInterface.SelectTarget
{
    using System;

    public class SelectTargetParameters
    {
        public bool CanCancel;
        public string Instructions;
        public Action<ITarget> TargetSelected;
        public Action<ITarget> TargetUnselected;
        public required ITriggerMessage TriggerMessage;
        public TargetValidator Validator;
        public int? X;
    }
}
