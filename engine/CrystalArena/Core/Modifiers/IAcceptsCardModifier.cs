namespace CrystalArena.Modifiers
{
    public interface IAcceptsCardModifier
    {
        void Accept(ICardModifier modifier);
    }
}
