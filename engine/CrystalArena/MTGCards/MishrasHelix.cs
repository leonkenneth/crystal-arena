namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.CostRules;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class MishrasHelix : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Mishra's Helix")
                .ManaCost("{5}")
                .Type("Artifact")
                .Text("{X},{T}: Tap X target backups.")
                .FlavorText(
                    "The helix was the finest example of Mishra's campaign strategy: if he couldn't have Argoth, no one could."
                )
                .ActivatedAbility(p =>
                {
                    p.Text = "{X},{T}: Tap X target backups.";

                    p.Cost = new AggregateCost(new PayMana(Mana.Zero, hasX: true), new Tap());

                    p.Effect = () => new TapTargets();

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Card(c => c.Is().Backup).On.Battlefield(),
                        trg =>
                        {
                            trg.MinCount = Value.PlusX;
                            trg.MaxCount = Value.PlusX;
                        }
                    );

                    p.TimingRule(new OnOpponentsTurn(Step.Upkeep));
                    p.TimingRule(new WhenOpponentControllsPermanents(x => x.Is().Backup));
                    p.CostRule(new XIsOpponentsBackupCount());
                    p.TargetingRule(new EffectTapBackup());
                });
        }
    }
}
