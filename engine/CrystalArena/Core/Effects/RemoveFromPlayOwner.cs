namespace CrystalArena.Effects
{
    public class RemoveFromPlayOwner : Effect
    {
        private Zone _owningCardZone;

        protected override void Initialize()
        {
            _owningCardZone = Source.OwningCard.Zone;
        }

        protected override void ResolveEffect()
        {
            Source.OwningCard.RemoveFromPlayFrom(_owningCardZone, this);
        }
    }
}
