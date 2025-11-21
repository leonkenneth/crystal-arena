namespace CrystalArena.AI.CostRules
{
    using System.Linq;

    public class XIsOpponentsBackupCount : CostRule
    {
        public override int CalculateX(CostRuleParameters p)
        {
            return p.Controller.Opponent.Battlefield.Backups.Count();
        }
    }
}
