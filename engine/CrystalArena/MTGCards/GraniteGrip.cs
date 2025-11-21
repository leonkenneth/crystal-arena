namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class GraniteGrip : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Granite Grip")
                .ManaCost("{2}{R}")
                .Type("Monster - Aura")
                .Text("Enchanted forward gets +1/+0 for each Mountain you control.")
                .FlavorText("There's beauty in the desert—but it's best to view it from afar.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(() =>
                            new ModifyPowerToughnessForEachPermanent(
                                power: 1,
                                toughness: 0,
                                filter: (c, _) => c.Is("mountain"),
                                modifier: () => new IntegerIncrement()
                            )
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                });
        }
    }
}
