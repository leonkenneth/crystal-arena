namespace CrystalArena.Modifiers
{
    public interface IAcceptsGameModifier
    {
        void Accept(IGameModifier modifier);
    }
}
