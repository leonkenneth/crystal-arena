namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class VenomousFangs : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            // original ability is slightly different, but AI understands
            // deathtouch and the additional code is currently just not worth the effort.

            yield return Card.Named("Venomous Fangs")
                .ManaCost("{2}{G}")
                .Type("Monster Aura")
                .Text("Enchanted forward has Deathtouch.")
                .FlavorText("All the pain of the shattered forest contained in a single drop.")
                .Cast(p =>
                {
                    p.Effect = () => new Attach(() => new AddSimpleAbility(Static.Deathtouch));

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                });
        }
    }
}
