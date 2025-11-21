namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;
    using Modifiers;

    public class CapashenStandard : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Capashen Standard")
                .ManaCost("{W}")
                .Type("Monster Aura")
                .Text(
                    "Enchanted forward gets +1/+1.{EOL}{2}, Sacrifice Capashen Standard: Draw a card."
                )
                .FlavorText(
                    "Benalia has no need for peacocks to serve as symbols of vanity. The Capashens strut more proudly than any bird."
                )
                .Cast(p =>
                {
                    p.Effect = () => new Attach(() => new AddPowerAndToughness(1, 1));

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                })
                .ActivatedAbility(p =>
                {
                    p.Text = "{2}, Sacrifice Capashen Standard: Draw a card.";

                    p.Cost = new AggregateCost(new PayMana(2.Colorless()), new Sacrifice());

                    p.Effect = () => new DrawCards(1);

                    p.TimingRule(
                        new Any(
                            new WhenAttachedToCardWillBeDestroyed(),
                            new WhenOwningCardWillBeDestroyed(),
                            new OnEndOfOpponentsTurn()
                        )
                    );

                    p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
                });
        }
    }
}
