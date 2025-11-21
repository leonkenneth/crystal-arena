namespace CrystalArena.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AI;
    using Decisions;

    public class ModalEffect : Effect
    {
        private readonly List<Action<AbilityParameters>> _effectAbilityParameterFactories;
        private readonly int _minCount;
        private readonly DynParam<int> _maxCount;

        private ModalEffect() { }

        public ModalEffect(
            DynParam<int> maxCount,
            params Action<AbilityParameters>[] effectAbilityParameterFactories
        )
        {
            _effectAbilityParameterFactories = effectAbilityParameterFactories.ToList();
            _minCount = 0;
            _maxCount = maxCount;

            RegisterDynamicParameters(maxCount);
        }

        public List<Action<AbilityParameters>> ChildEffectFactories =>
            _effectAbilityParameterFactories;

        public List<ModalEffectParameters> ChildEffects
        {
            get
            {
                return ChildEffectFactories
                    .Select(factory =>
                    {
                        var parameters = new ModalEffectParameters();
                        factory(parameters);
                        return parameters;
                    })
                    .ToList();
            }
        }

        protected override void ResolveEffect()
        {
            // A modal effect cannot be resolved directly, only one or more of its child effect can
            throw new NotImplementedException();
        }
    }
}
