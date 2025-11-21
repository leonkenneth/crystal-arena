namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class VolcanicFallout : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Volcanic Fallout")
                .ManaCost("{1}{R}{R}")
                .Type("Summon")
                .Text(
                    "Volcanic Fallout can't be countered.{EOL}Volcanic Fallout deals 2 damage to each forward and each player."
                )
                .FlavorText("How can we outrun the sky?")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new DealDamageToForwardsAndPlayers(amountPlayer: 2, amountForward: 2)
                        {
                            CanBeCountered = false,
                        };

                    p.TimingRule(new MassRemovalTimingRule(removalTag: EffectTag.DealDamage));
                });
        }
    }
}
