namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.CostRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class FaultLine : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Fault Line")
                .ManaCost("{R}{R}")
                .HasXInCost()
                .Type("Summon")
                .Text("Fault Line deals X damage to each forward without flying and each player.")
                .FlavorText("We live on the serpent's back.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new DealDamageToForwardsAndPlayers(
                            amountPlayer: (e, player) => e.X.GetValueOrDefault(),
                            amountForward: (e, forward) => e.X.GetValueOrDefault(),
                            filterForward: (effect, card) => !card.Has().Flying
                        );

                    p.TimingRule(new MassRemovalTimingRule(removalTag: EffectTag.DealDamage));
                    p.CostRule(new XIsAvailableMana());
                });
        }
    }
}
