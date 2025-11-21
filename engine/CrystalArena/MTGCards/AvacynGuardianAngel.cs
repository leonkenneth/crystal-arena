namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using Costs;
    using Effects;

    public class AvacynGuardianAngel : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Avacyn, Guardian Angel")
                .ManaCost("{2}{W}{W}{W}")
                .Type("Legendary Forward — Angel")
                .Text(
                    "{Flying}, {brave}{EOL}{1}{W}: Prevent all damage that would be dealt to another target forward this turn by sources of the color of your choice.{EOL}{5}{W}{W}: Prevent all damage that would be dealt to target player this turn by sources of the color of your choice."
                )
                .Power(5)
                .Toughness(4)
                .SimpleAbilities(Static.Flying, Static.Brave)
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{1}{W}: Prevent all damage that would be dealt to another target forward this turn by sources of the color of your choice.";
                    p.Cost = new PayMana("{1}{W}".Parse());

                    p.Effect = () => new PreventAllDamageToTargetsFromChosenColor();

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Forward(canTargetSelf: false).On.Battlefield(),
                        trg =>
                        {
                            trg.MinCount = 1;
                            trg.MaxCount = 1;
                        }
                    );

                    p.TargetingRule(new EffectPreventNextDamageToTargets());
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{5}{W}{W}: Prevent all damage that would be dealt to target player this turn by sources of the color of your choice.";
                    p.Cost = new PayMana("{5}{W}{W}".Parse());

                    p.Effect = () => new PreventAllDamageToTargetsFromChosenColor();

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Player(),
                        trg =>
                        {
                            trg.MinCount = 1;
                            trg.MaxCount = 1;
                        }
                    );

                    p.TargetingRule(new EffectPreventNextDamageToTargets());
                });
        }
    }
}
