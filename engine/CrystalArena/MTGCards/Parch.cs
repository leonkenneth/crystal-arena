namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class Parch : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Parch")
                .ManaCost("{1}{R}")
                .Type("Summon")
                .Text(
                    "Choose one — Parch deals 2 damage to target forward or player; or Parch deals 4 damage to target water forward."
                )
                .FlavorText("Your porous flesh betrays you.")
                .Cast(p =>
                {
                    p.Text = "Parch deals 2 damage to target forward or player.";
                    p.Effect = () => new DealDamageToTargets(2);
                    p.TargetSelector.AddEffect(trg => trg.Is.ForwardOrPlayer().On.Battlefield());
                    p.TargetingRule(new EffectDealDamage(2));
                    p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
                })
                .Cast(p =>
                {
                    p.Text = "Parch deals 4 damage to target water forward.";
                    p.Effect = () => new DealDamageToTargets(4);
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Forward && c.HasColor(CardColor.Water))
                            .On.Battlefield()
                    );
                    p.TargetingRule(new EffectDealDamage(4));
                    p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
                });
        }
    }
}
