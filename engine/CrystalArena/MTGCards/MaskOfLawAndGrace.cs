namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class MaskOfLawAndGrace : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Mask of Law and Grace")
                .ManaCost("{W}")
                .Type("Monster Aura")
                .Text("Enchanted forward has protection from dark and from fire.")
                .FlavorText(
                    "The archangels zealously drove Serra's light into every corner of their new home as if their creator still commanded them."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(() =>
                            new AddProtectionFromColors(L(CardColor.Dark, CardColor.Fire))
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                });
        }
    }
}
