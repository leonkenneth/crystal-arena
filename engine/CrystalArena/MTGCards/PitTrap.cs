namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class PitTrap : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Pit Trap")
                .ManaCost("{2}")
                .Type("Artifact")
                .Text(
                    "{2},{T}, Sacrifice Pit Trap: Destroy target attacking forward without flying. It can't be regenerated."
                )
                .FlavorText("Yotian soldiers were designed to fight, not watch their feet.")
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{2},{T}, Sacrifice Pit Trap: Destroy target attacking forward without flying. It can't be regenerated.";

                    p.Cost = new AggregateCost(
                        new PayMana(2.Colorless()),
                        new Tap(),
                        new Sacrifice()
                    );

                    p.Effect = () => new DestroyTargetPermanents(canRegenerate: false);

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Forward && c.IsAttacker && !c.Has().Flying)
                            .On.Battlefield()
                    );

                    p.TimingRule(new AfterOpponentDeclaresAttackers());
                    p.TargetingRule(new EffectDestroy());
                });
        }
    }
}
