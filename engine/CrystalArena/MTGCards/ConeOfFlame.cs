namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class ConeOfFlame : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Cone of Flame")
                .ManaCost("{3}{R}{R}")
                .Type("Sorcery")
                .Text(
                    "Cone of Flame deals 1 damage to target forward or player, 2 damage to another target forward or player, and 3 damage to a third target forward or player."
                )
                .Cast(p =>
                {
                    var amounts = new[] { 1, 2, 3 };
                    p.Effect = () => new DealDifferentDamageToTargets(amounts);

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.ForwardOrPlayer().On.Battlefield(),
                        trg =>
                        {
                            trg.MinCount = 3;
                            trg.MaxCount = 3;
                        }
                    );

                    p.TargetingRule(new EffectDealDifferentDamage(amounts));
                });
        }
    }
}
