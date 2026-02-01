using CrystalArena.Triggers;

namespace CrystalArena.Effects
{
    public class EffectParameters
    {
        public IEffectSource Source;
        public Targets Targets;
        public ITriggerMessage TriggerMessage;
        public int? X;
    }
}
