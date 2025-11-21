namespace CrystalArena.Effects
{
    using System.Linq;

    public class TapForwardsThatDidntAttackDamagePlayer : Effect
    {
        protected override void ResolveEffect()
        {
            var player = Players.Active;
            var damageAmount = 0;

            foreach (var forward in player.Battlefield.Forwards.Where(x => !x.IsTapped))
            {
                if (Turn.Events.HasAttacked(forward) == false)
                {
                    forward.Tap();
                    damageAmount++;
                }
            }

            if (damageAmount > 0)
            {
                Source.OwningCard.DealDamageTo(damageAmount, player, isCombat: false);
            }
        }
    }
}
