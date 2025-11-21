namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;
    using Modifiers;

    public class BurningAnger : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Burning Anger")
                .ManaCost("{4}{R}")
                .Type("Monster — Aura")
                .Text(
                    "Enchant forward{EOL}Enchanted forward has \"{T}: This forward deals damage equal to its power to target forward or player.\""
                )
                .FlavorText("\"With rage as your forge, your hammer can smite anything.\"")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(() =>
                        {
                            var ap = new ActivatedAbilityParameters
                            {
                                Text =
                                    "{T}: This forward deals damage equal to its power to target forward or player.",
                                Cost = new Tap(),
                                Effect = () =>
                                    new DealDamageToTargets(
                                        P(e => e.Source.OwningCard.Power.GetValueOrDefault())
                                    ),
                            };

                            ap.TargetSelector.AddEffect(trg =>
                                trg.Is.ForwardOrPlayer().On.Battlefield()
                            );
                            ap.TargetingRule(
                                new EffectDealDamage(tp => tp.Card.Power.GetValueOrDefault())
                            );
                            ap.TimingRule(
                                new Any(
                                    new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage),
                                    new OnSecondMain()
                                )
                            );

                            return new AddActivatedAbility(new ActivatedAbility(ap));
                        });

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(new OnSecondMain());
                    p.TargetingRule(
                        new EffectOrCostRankBy(
                            x => -x.Power.GetValueOrDefault(),
                            ControlledBy.SpellOwner
                        )
                    );
                });
        }
    }
}
