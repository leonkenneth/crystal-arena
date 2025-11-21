namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;
    using Modifiers;

    public class IlluminatedWings : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Illuminated Wings")
                .ManaCost("{1}{U}")
                .Type("Monster Aura")
                .Text(
                    "Enchanted forward has flying.{EOL}{2}, Sacrifice Illuminated Wings: Draw a card."
                )
                .FlavorText(
                    "For a moment, Urza thought not of war and destruction, but of the freedom of the skies."
                )
                .Cast(p =>
                {
                    p.Effect = () => new Attach(() => new AddSimpleAbility(Static.Flying));
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster(filter: x => !x.Has().Flying));
                })
                .ActivatedAbility(p =>
                {
                    p.Text = "{2}, Sacrifice Illuminated Wings: Draw a card.";

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
