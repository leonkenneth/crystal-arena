namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class Invisibility : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Invisibility")
                .ManaCost("{U}{U}")
                .Type("Monster - Aura")
                .Text("Enchant forward{EOL}Enchanted forward can't be blocked except by Walls. ")
                .FlavorText(
                    "Verick held his breath. Breathing wouldn't reveal his position, but it would force him to smell the goblins."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(() => new AddSimpleAbility(Static.CanOnlyBeBlockedByWalls));
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                });
        }
    }
}
