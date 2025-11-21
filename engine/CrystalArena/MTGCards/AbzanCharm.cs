namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class AbzanCharm : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Abzan Charm")
                .ManaCost("{W}{B}{G}")
                .Type("Summon")
                .Text(
                    "Choose one —{EOL}• RemoveFromPlay target forward with power 3 or greater.{EOL}• You draw two cards and you lose 2 life.{EOL}• Distribute two +1/+1 counters among one or two target forwards."
                )
                .Cast(p =>
                {
                    p.Text =
                        "{{W}}{{B}}{{G}}: RemoveFromPlay target forward with power 3 or greater.";
                    p.Effect = () => new RemoveFromPlayTargets();
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Forward && c.Power >= 3).On.Battlefield()
                    );
                    p.TargetingRule(new EffectRemoveFromPlayBattlefield());
                    p.TimingRule(new TargetRemovalTimingRule(EffectTag.RemoveFromPlay));
                })
                .Cast(p =>
                {
                    p.Text = "{{W}}{{B}}{{G}}: You draw two cards and you lose 2 life.";
                    p.Effect = () => new DrawCards(2, lifeloss: 2);
                    p.TimingRule(new OnEndOfOpponentsTurn());
                })
                .Cast(p =>
                {
                    p.Text =
                        "{{W}}{{B}}{{G}}: Distribute two +1/+1 counters among one or two target forwards.";

                    p.DistributeAmount = 2;
                    p.Effect = () =>
                        new DistributeCountersAmongTargets(() => new PowerToughness(1, 1));

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Forward().On.Battlefield(),
                        trg =>
                        {
                            trg.MinCount = 1;
                            trg.MaxCount = 2;
                        }
                    );

                    p.TargetingRule(
                        new EffectOrCostRankBy(c => -c.Score, controlledBy: ControlledBy.SpellOwner)
                    );

                    p.TimingRule(new OnFirstMain());
                });
        }
    }
}
