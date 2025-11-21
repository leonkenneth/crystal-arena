namespace CrystalArena.Costs
{
    public class SacrificeThis : Cost
    {
        public override CanPayResult CanPayPartial(bool needsToPayManaCost)
        {
            return Card.Zone == Zone.Battlefield;
        }

        public override void PayPartial(PayCostParameters p)
        {
            Card.Sacrifice();
        }
    }
}
