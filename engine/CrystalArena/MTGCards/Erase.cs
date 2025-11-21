namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Erase : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Erase")
                .ManaCost("{W}")
                .Type("Summon")
                .Text("RemoveFromPlay target monster.")
                .FlavorText("Perception is more pleasing than truth.")
                .Cast(p =>
                {
                    p.Effect = () => new RemoveFromPlayTargets();
                    p.TargetSelector.AddEffect(trg => trg.Is.Monster().On.Battlefield());
                    p.TargetingRule(new EffectRemoveFromPlayBattlefield());
                    p.TimingRule(new TargetRemovalTimingRule(EffectTag.RemoveFromPlay));
                });
        }
    }
}
