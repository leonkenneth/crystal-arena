namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class SeasonedMarshal : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Seasoned Marshal")
                .ManaCost("{2}{W}{W}")
                .Type("Forward Human Soldier")
                .Text("Whenever Seasoned Marshal attacks, you may tap target forward.")
                .FlavorText(
                    "There are only two rules of tactics: never be without a plan, and never rely on it."
                )
                .Power(2)
                .Toughness(2)
                .TriggeredAbility(p =>
                {
                    p.Text = "Whenever Seasoned Marshal attacks, you may tap target forward.";
                    p.Trigger(new WhenThisAttacks());
                    p.Effect = () => new TapTargets();

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Forward().On.Battlefield(),
                        trg =>
                        {
                            trg.MinCount = 0;
                            trg.MaxCount = 1;
                        }
                    );

                    p.TargetingRule(new EffectTapForward());
                });
        }
    }
}
