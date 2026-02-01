using CrystalArena.Triggers;

namespace CrystalArena.Effects
{
    using System.Collections.Generic;

    public interface IEffectSource
    {
        Card OwningCard { get; }
        Card SourceCard { get; }
        bool HasExBurst { get; }

        void EffectCountered(SpellCounterReason reason);
        void EffectPushedOnStack();
        void EffectResolved(Effect.Context ctx);

        bool IsTargetStillValid(ITarget target, ITriggerMessage triggerMessage = null);
        bool ValidateTargetDependencies(List<ITarget> costTargets, List<ITarget> effectTargets);
    }
}
