namespace CrystalArena.Modifiers
{
    public class ManualLifetime : Lifetime
    {
        public void EndLife()
        {
            End();
        }
    }
}
