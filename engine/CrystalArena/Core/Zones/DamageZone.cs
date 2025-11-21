namespace CrystalArena
{
    public class DamageZone : OrderedZone, IZoneQuery
    {
        public DamageZone(Player owner)
            : base(owner) { }

        private DamageZone()
        {
            /* for state copy */
        }

        public override Zone Name
        {
            get { return Zone.DamageZone; }
        }
    }
}
