namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class TreacherousLink : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Treacherous Link")
                .ManaCost("{1}{B}")
                .Type("Monster - Aura")
                .Text(
                    "All damage that would be dealt to enchanted forward is dealt to its controller instead."
                )
                .FlavorText("You cannot possibly know the toll your alliances will exact from you.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(modifiers: () =>
                            new AddDamageRedirection(
                                modifier => new RedirectDamageFromTargetToTarget(
                                    @from: modifier.SourceCard.AttachedTo,
                                    to: modifier.SourceCard.AttachedTo.Controller
                                )
                            )
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectRedirectDamageToControllerMonster());
                });
        }
    }
}
