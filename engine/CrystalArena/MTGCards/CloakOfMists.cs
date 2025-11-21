namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class CloakOfMists : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Cloak of Mists")
                .ManaCost("{1}{U}")
                .Type("Monster Aura")
                .Text("Enchanted forward is unblockable.")
                .FlavorText(
                    "All we could lose, we did. All we could keep, we do. And both are shrouded by mists."
                )
                .Cast(p =>
                {
                    p.Effect = () => new Attach(() => new AddSimpleAbility(Static.Unblockable));
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster(filter: x => !x.Has().Unblockable));
                });
        }
    }
}
