namespace CrystalArena
{
    using System.Linq;
    using System.Security.Policy;
    using CrystalArena.Infrastructure;

    public class LimitBreak : UnorderedZone, IZoneQuery
    {
        public LimitBreak(Player owner)
            : base(owner) { }

        private LimitBreak()
        {
            /* for state copy */
        }

        public override Zone Name
        {
            get { return Zone.LimitBreak; }
        }

        public override int CalculateHash(HashCalculator calc)
        {
            var visible = this.Where(x => x.IsVisibleToPlayer(Owner)).ToList();

            if (visible.Count == 0)
                return Count;

            return HashCalculator.Combine(Count, calc.Calculate(visible, true));
        }
    }
}
