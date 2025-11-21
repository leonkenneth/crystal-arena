namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class GoblinMedics : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Goblin Medics")
                .ManaCost("{2}{R}")
                .Type("Forward Goblin Shaman")
                .Text(
                    "Whenever Goblin Medics becomes tapped, it deals 1 damage to target forward or player."
                )
                .FlavorText("First, do some harm.")
                .Power(1)
                .Toughness(1)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever Goblin Medics becomes tapped, it deals 1 damage to target forward or player.";
                    p.Trigger(new OnOwnerGetsTapped());
                    p.Effect = () => new DealDamageToTargets(1);
                    p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());
                    p.TargetingRule(new EffectDealDamage(1));
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
