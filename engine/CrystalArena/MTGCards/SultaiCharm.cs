namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class SultaiCharm : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Sultai Charm")
                .ManaCost("{B}{G}{U}")
                .Type("Summon")
                .Text(
                    "Choose one —{EOL}• Destroy target monocolored forward.{EOL}• Destroy target artifact or monster.{EOL}• Draw two cards, then discard a card."
                )
                .Cast(p =>
                {
                    p.Text = "{{B}}{{G}}{{U}}: Destroy target monocolored forward.";
                    p.Effect = () => new DestroyTargetPermanents();
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Forward && !c.IsMultiColored).On.Battlefield()
                    );
                    p.TargetingRule(new EffectDestroy());
                    p.TimingRule(
                        new TargetRemovalTimingRule().RemovalTags(
                            EffectTag.Destroy,
                            EffectTag.ForwardsOnly
                        )
                    );
                })
                .Cast(p =>
                {
                    p.Text = "{{B}}{{G}}{{U}}: Destroy target artifact or monster.";
                    p.Effect = () => new DestroyTargetPermanents();
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(card => card.Is().Artifact || card.Is().Monster)
                            .On.Battlefield()
                    );

                    p.TargetingRule(new EffectDestroy());
                    p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Destroy));
                })
                .Cast(p =>
                {
                    p.Text = "{{B}}{{G}}{{U}}: Draw two cards, then discard a card.";
                    p.Effect = () => new DrawCards(2, discardCount: 1);
                    p.TimingRule(new OnEndOfOpponentsTurn());
                });
        }
    }
}
