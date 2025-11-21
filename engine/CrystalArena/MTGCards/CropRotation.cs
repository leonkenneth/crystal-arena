namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;

    public class CropRotation : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Crop Rotation")
                .ManaCost("{G}")
                .Type("Summon")
                .Text(
                    "As an additional cost to cast Crop Rotation, sacrifice a backup.{EOL}Search your library for a backup card and put that card onto the battlefield. Then shuffle your library."
                )
                .FlavorText("Hmm . . . maybe lotuses this year.")
                .Cast(p =>
                {
                    p.Cost = new AggregateCost(new PayMana(Mana.Wind), new Sacrifice());

                    p.Effect = () =>
                        new SearchMainDeckPutToZone(
                            zone: Zone.Battlefield,
                            minCount: 0,
                            maxCount: 1,
                            validator: (c, ctx) => c.Is().Backup,
                            text: "Search your library for a backup card."
                        );

                    p.TargetSelector.AddCost(
                        trg =>
                            trg.Is.Card(c => c.Is().Backup, ControlledBy.SpellOwner)
                                .On.Battlefield(),
                        trg => trg.Message = "Select a backup to sacrifice."
                    );

                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new CostSacrificeBackupToSearchBackup());
                });
        }
    }
}
