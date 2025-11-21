namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;
    using Modifiers;

    public class InfernoFist : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Inferno Fist")
                .ManaCost("{1}{R}")
                .Type("Monster — Aura")
                .Text(
                    "Enchant forward you control{EOL}Enchanted forward gets +2/+0.{EOL}{R}, Sacrifice Inferno Fist: Inferno Fist deals 2 damage to target forward or player."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(() => new AddPowerAndToughness(2, 0)).SetTags(
                            EffectTag.IncreasePower
                        );

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Forward(controlledBy: ControlledBy.SpellOwner).On.Battlefield()
                    );

                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{R}, Sacrifice Inferno Fist: Inferno Fist deals 2 damage to target forward or player.";

                    p.Cost = new AggregateCost(new PayMana(Mana.Fire), new Sacrifice());

                    p.Effect = () => new DealDamageToTargets(2);
                    p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());
                    p.TargetingRule(new EffectDealDamage(2));
                    p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
                });
        }
    }
}
