namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class ZephidsEmbrace : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Zephid's Embrace")
                .ManaCost("{2}{U}{U}")
                .Type("Monster - Aura")
                .Text("Enchanted forward gets +2/+2 and has flying and shroud.")
                .FlavorText("Spells will shun you, as will everyone else.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(
                            () => new AddPowerAndToughness(2, 2),
                            () => new AddSimpleAbility(Static.Flying),
                            () => new AddSimpleAbility(Static.Shroud)
                        ).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                });
        }
    }
}
