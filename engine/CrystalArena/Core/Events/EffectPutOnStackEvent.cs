using CrystalArena.Triggers;

namespace CrystalArena.Events
{
    public class EffectPutOnStackEvent : ITriggerMessage
    {
        public readonly Effect Effect;

        public EffectPutOnStackEvent(Effect effect)
        {
            Effect = effect;
        }
    }
}
