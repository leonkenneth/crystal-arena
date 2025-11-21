namespace CrystalArena.Effects
{
    public class ShuffleOwningCardIntoMainDeck : Effect
    {
        protected override void ResolveEffect()
        {
            Source.OwningCard.ShuffleIntoMainDeck();
        }
    }
}
