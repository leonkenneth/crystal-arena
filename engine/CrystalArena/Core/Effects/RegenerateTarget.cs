namespace CrystalArena.Effects
{
    using CrystalArena.AI;

    public class RegenerateTarget : Effect
    {
        public RegenerateTarget()
        {
            SetTags(EffectTag.Regenerate);
        }

        protected override void ResolveEffect()
        {
            Target.Card().HasRegenerationShield = true;
        }
    }
}
