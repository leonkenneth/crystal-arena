namespace CrystalArena
{
    using System.Linq;

    public class Hand : UnorderedZone, IHandQuery
    {
        public Hand(Player owner)
            : base(owner) { }

        private Hand()
        {
            /* for state copy */
        }

        public int Score
        {
            get { return this.Sum(x => x.Score); }
        }
        public override Zone Name
        {
            get { return Zone.Hand; }
        }
    }
}
