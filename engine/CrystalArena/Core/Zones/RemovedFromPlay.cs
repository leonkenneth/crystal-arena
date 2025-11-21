namespace CrystalArena
{
    public class RemovedFromPlay : UnorderedZone, IZoneQuery
    {
        public RemovedFromPlay(Player owner)
            : base(owner) { }

        private RemovedFromPlay()
        {
            /* for state copy */
        }

        public override Zone Name
        {
            get { return Zone.RemovedFromPlay; }
        }
    }
}
