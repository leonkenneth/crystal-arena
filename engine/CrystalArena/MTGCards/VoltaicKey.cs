namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class VoltaicKey : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Voltaic Key")
                .ManaCost("{1}")
                .Type("Artifact")
                .Text("{1},{T}: Untap target artifact.")
                .FlavorText("The key did not work on a single lock, yet it opened many doors.")
                .ActivatedAbility(p =>
                {
                    p.Text = "{1},{T}: Untap target artifact.";

                    p.Cost = new AggregateCost(new PayMana(1.Colorless()), new Tap());

                    p.Effect = () => new UntapTargetPermanents();
                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.Card(c => c.Is().Artifact).On.Battlefield()
                    );

                    p.TimingRule(new OnMainStepsOfYourTurn());
                    p.TargetingRule(
                        new EffectOrCostRankBy(c => -c.Score, ControlledBy.SpellOwner)
                        {
                            ConsiderTargetingSelf = false,
                        }
                    );
                });
        }
    }
}
