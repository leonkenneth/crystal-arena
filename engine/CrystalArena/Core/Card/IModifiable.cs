namespace CrystalArena
{
    using Modifiers;

    public interface IModifiable
    {
        void RemoveModifier(IModifier modifier);
    }
}
