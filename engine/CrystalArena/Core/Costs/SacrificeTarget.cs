namespace CrystalArena.Costs
{
    using System.Linq;

    public class SacrificeTarget : Cost
    {
        public override CanPayResult CanPayPartial(bool needsToPayManaCost)
        {
            return Controller.Battlefield.Any(permanent =>
                permanent != Card && Validator.IsTargetValid(permanent)
            );
        }

        public override void PayPartial(PayCostParameters p)
        {
            var target = p.Targets.Cost.FirstOrDefault();
            if (target != null)
            {
                target.Card().Sacrifice();
            }
        }
    }
}
