namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class DevouringLight : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Devouring Light")
                .ManaCost("{1}{W}{W}")
                .Type("Summon")
                .Text(
                    "{Convoke}{I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}RemoveFromPlay target attacking or blocking forward."
                )
                .FlavorText("\"Even your shadow is too foul to tolerate.\"")
                .SimpleAbilities(Static.Convoke)
                .Cast(p =>
                {
                    p.Text = "RemoveFromPlay target attacking or blocking forward.";

                    p.Effect = () => new RemoveFromPlayTargets();

                    p.TargetSelector.AddEffect(
                        trg =>
                            trg.Is.Card(c => c.Is().Forward && (c.IsAttacker || c.IsBlocker))
                                .On.Battlefield(),
                        trg =>
                        {
                            trg.Message = "Select target attacking or blocking forward.";
                        }
                    );

                    p.TimingRule(
                        new Any(new AfterYouDeclareBlockers(), new AfterOpponentDeclaresBlockers())
                    );
                    p.TargetingRule(new EffectRemoveFromPlayBattlefield());
                });
        }
    }
}
