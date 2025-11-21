namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.CostRules;
    using AI.TimingRules;
    using CrystalArena.AI.TargetingRules;
    using Effects;
    using Modifiers;

    public class ReturnToTheRanks : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Return to the Ranks")
                .ManaCost("{W}{W}")
                .HasXInCost()
                .Type("Sorcery")
                .Text(
                    "{Convoke} {I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for or one mana of that forward's color.){/I}{EOL}Return X target forward cards with converted mana cost 2 or less from your breakZone to the battlefield. "
                )
                .SimpleAbilities(Static.Convoke)
                .Cast(p =>
                {
                    p.Effect = () => new PutTargetsToBattlefield();

                    p.TargetSelector.AddEffect(
                        trg =>
                            trg.Is.Card(c => c.Is().Forward && c.ConvertedCost <= 2)
                                .In.YourBreakZone(),
                        trg =>
                        {
                            trg.MinCount = Value.PlusX;
                            trg.MaxCount = Value.PlusX;
                        }
                    );

                    p.CostRule(new XIsAvailableMana());
                    p.TargetingRule(new EffectOrCostRankBy(c => -c.Score));
                    p.TimingRule(
                        new WhenYourBreakZoneCountIs(
                            minCount: 1,
                            selector: c => c.Is().Forward && c.ConvertedCost <= 2
                        )
                    );
                });
        }
    }
}
