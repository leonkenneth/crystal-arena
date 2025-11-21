namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using Effects;
    using Modifiers;

    public class DebilitatingInjury : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Debilitating Injury")
                .ManaCost("{1}{B}")
                .Type("Monster - Aura")
                .Text("Enchant forward{EOL}Enchanted forward gets -2/-2.")
                .FlavorText(
                    "\"If weakness does not exist within the Temur then we shall force it upon them.\"{EOL}—Sidisi, khan of the Sultai"
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(() => new AddPowerAndToughness(-2, -2))
                        {
                            ToughnessReduction = 2,
                        };
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TargetingRule(new EffectReduceToughness(2));
                });
        }
    }
}
