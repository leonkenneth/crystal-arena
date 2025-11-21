namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Befoul : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Befoul")
                .ManaCost("{2}{B}{B}")
                .Type("Sorcery")
                .Text("Destroy target backup or nonblack forward. It can't be regenerated.")
                .FlavorText(
                    "The backup putrefied at its touch, turned into an oily bile in seconds."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new DestroyTargetPermanents(canRegenerate: false).SetTags(
                            EffectTag.Destroy,
                            EffectTag.CannotRegenerate
                        );

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(card =>
                                card.Is().Backup
                                || (card.Is().Forward && !card.HasColor(CardColor.Dark))
                            )
                            .On.Battlefield()
                    );
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectDestroy());
                });
        }
    }
}
