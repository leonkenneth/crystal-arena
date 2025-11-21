namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class DivineVerdict : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Divine Verdict")
                .ManaCost("{3}{W}")
                .Type("Summon")
                .Text("Destroy target attacking or blocking forward.")
                .FlavorText("\"Guilty.\"")
                .Cast(p =>
                {
                    p.Effect = () => new DestroyTargetPermanents();

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
                        new Any(
                            new AfterOpponentDeclaresAttackers(),
                            new AfterOpponentDeclaresBlockers()
                        )
                    );
                    p.TargetingRule(new EffectDestroy());
                });
        }
    }
}
