namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class Pacifism : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Pacifism")
                .ManaCost("{1}{W}")
                .Type("Monster - Aura")
                .Text("Enchanted forward can't attack or block.")
                .FlavorText(
                    "Fight? I cannot. I do not care if I live or die, so long as I can rest."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(
                            () => new AddSimpleAbility(Static.CannotBlock),
                            () => new AddSimpleAbility(Static.CannotAttack)
                        ).SetTags(EffectTag.CombatDisabler);

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCannotBlockAttack());
                });
        }
    }
}
