namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;

    public class TyrantsMachine : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Tyrant's Machine")
                .ManaCost("{2}")
                .Type("Artifact")
                .Text("{4},{T}: Tap target forward.")
                .FlavorText(
                    "\"Though tempered differently, all wills can be broken.\"{EOL}—Inquisitor Kyrik"
                )
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .ActivatedAbility(p =>
                {
                    p.Text = "{4},{T}: Tap target forward.";

                    p.Cost = new AggregateCost(new PayMana(4.Colorless()), new Tap());

                    p.Effect = () => new TapTargets();
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnStep(Step.BeginningOfCombat));
                    p.TargetingRule(new EffectTapForward());
                });
        }
    }
}
