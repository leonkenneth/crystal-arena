namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class GhituFireEater : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Ghitu Fire-Eater")
                .ManaCost("{2}{R}")
                .Type("Forward Human Nomad")
                .Text(
                    "{T}, Sacrifice Ghitu Fire-Eater: Ghitu Fire-Eater deals damage equal to its power to target forward or player."
                )
                .FlavorText("They are called 'Mi'uto,' which means 'one use'.")
                .Power(2)
                .Toughness(2)
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{T}, Sacrifice Ghitu Fire-Eater: Ghitu Fire-Eater deals damage equal to its power to target forward or player.";

                    p.Cost = new AggregateCost(new Tap(), new Sacrifice());

                    p.Effect = () =>
                        new DealDamageToTargets(
                            P(e => e.Source.OwningCard.Power.GetValueOrDefault())
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());
                    p.TargetingRule(new EffectDealDamage(tp => tp.Card.Power.GetValueOrDefault()));
                });
        }
    }
}
