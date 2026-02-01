using CrystalArena.Triggers;

namespace CrystalArena.Events
{
    public class EffectResolvedEvent : ITriggerMessage
    {
        public readonly Effect Effect;

        public EffectResolvedEvent(Effect effect)
        {
            Effect = effect;
        }
    }
}
