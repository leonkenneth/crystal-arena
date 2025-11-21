namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using Effects;

    public class HuntTheWeak : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hunt the Weak")
                .ManaCost("{3}{G}")
                .Type("Sorcery")
                .Text(
                    "Put a +1/+1 counter on target forward you control. Then that forward fights target forward you don't control. {I}(Each deals damage equal to its power to the other.){/I}"
                )
                .FlavorText("He who hesitates is lunch.")
                .Cast(p =>
                {
                    p.Text =
                        "Put a +1/+1 counter on target forward you control. Then that forward fights target forward you don't control.";
                    p.Effect = () =>
                        new PutCounterOnYoursAndFightWithOpponentsForward(
                            () => new PowerToughness(1, 1),
                            1
                        ).SetTags(EffectTag.Bounce);

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Forward(ControlledBy.SpellOwner).On.Battlefield(),
                        trg =>
                        {
                            trg.Message = "Select target forward you control.";
                        }
                    );

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Forward(ControlledBy.Opponent).On.Battlefield(),
                        trg =>
                        {
                            trg.Message = "Select target forward your opponent controls.";
                        }
                    );

                    p.TargetingRule(new EffectForwardsDealsDamageEqualToPowerToEachOther(1, 1));
                });
        }
    }
}
