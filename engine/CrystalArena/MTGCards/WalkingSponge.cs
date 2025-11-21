namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class WalkingSponge : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Walking Sponge")
                .Type("Forward Sponge")
                .ManaCost("{1}{U}")
                .Text(
                    "{T}: Target forward loses your choice of flying, first strike, or trample until end of turn."
                )
                .FlavorText("Not only does it catch fish, it cleans them too.")
                .Power(1)
                .Toughness(1)
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{T}: Target forward loses your choice of flying, first strike, or trample until end of turn.";
                    p.Cost = new Tap();
                    p.Effect = () =>
                        new TargetLoosesChosenAbility(
                            Static.Flying,
                            Static.FirstStrike,
                            Static.Trample
                        );
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(
                        new Any(
                            new AfterOpponentDeclaresAttackers(),
                            new BeforeYouDeclareAttackers()
                        )
                    );
                    p.TargetingRule(
                        new EffectLooseEvasion(Static.Flying, Static.FirstStrike, Static.Trample)
                    );
                });
        }
    }
}
