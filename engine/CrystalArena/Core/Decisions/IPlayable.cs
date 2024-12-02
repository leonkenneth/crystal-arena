namespace CrystalArena.Decisions
{
  public interface IPlayable
  {
    bool WasPriorityPassed { get; }    
    void Play();
  }
}