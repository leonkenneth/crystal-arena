namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;

    public class Attrition : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Attrition")
                .ManaCost("{1}{B}{B}")
                .Type("Monster")
                .Text("{B}, Sacrifice a forward: Destroy target nonblack forward.")
                .FlavorText(
                    "I will trade life for life with the insurgents. Our resources, unlike theirs, are limitless."
                )
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .ActivatedAbility(p =>
                {
                    p.Text = "{B}, Sacrifice a forward: Destroy target nonblack forward.";
                    p.Cost = new AggregateCost(new PayMana(Mana.Dark), new Sacrifice());

                    p.Effect = () => new DestroyTargetPermanents();

                    p.TargetSelector.AddCost(
                        trg =>
                            trg.Is.Card(c => c.Is().Forward, ControlledBy.SpellOwner)
                                .On.Battlefield(),
                        trg => trg.Message = "Select a forward to sacrifice."
                    );

                    p.TargetSelector.AddEffect(
                        trg =>
                            trg.Is.Card(c => c.Is().Forward && !c.HasColor(CardColor.Dark))
                                .On.Battlefield(),
                        trg => trg.Message = "Select a forward to destroy."
                    );

                    p.TargetingRule(new CostSacrificeEffectDestroy());

                    p.TimingRule(
                        new TargetRemovalTimingRule().RemovalTags(
                            EffectTag.Destroy,
                            EffectTag.ForwardsOnly
                        )
                    );
                });
        }
    }
}
