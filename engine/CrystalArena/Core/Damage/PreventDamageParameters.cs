namespace CrystalArena
{
    public class PreventDamageParameters
    {
        public int Amount;
        public bool IsCombat;
        public bool CanBePrevented = true;
        public bool QueryOnly = true;
        public IDamageSource Source;
        public ITarget Target;
    }
}
