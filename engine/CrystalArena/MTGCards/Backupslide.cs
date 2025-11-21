namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class Backupslide : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Backupslide")
                .ManaCost("{R}")
                .Type("Sorcery")
                .Text(
                    "Sacrifice any number of Mountains. Backupslide deals that much damage to target player."
                )
                .FlavorText("Sometimes the mountain takes.")
                .Cast(p =>
                {
                    p.Effect = () => new SacrificeToDealDamageToTarget(c => c.Is("mountain"));
                    p.TargetSelector.AddEffect(trg => trg.Is.Player());

                    p.TimingRule(new OnSecondMain());
                    p.TimingRule(new WhenYouHavePermanents(c => c.Is("mountain")));
                    p.TargetingRule(new EffectOpponent());
                });
        }
    }
}
