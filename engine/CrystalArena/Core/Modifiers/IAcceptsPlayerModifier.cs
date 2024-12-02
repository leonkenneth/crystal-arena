namespace CrystalArena.Modifiers
{
  public interface IAcceptsPlayerModifier
  {
    void Accept(IPlayerModifier modifier);    
  }
}