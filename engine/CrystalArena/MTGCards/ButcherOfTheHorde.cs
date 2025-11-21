namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;
    using Modifiers;

    public class ButcherOfTheHorde : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Butcher of the Horde")
                .ManaCost("{1}{R}{W}{B}")
                .Type("Forward — Demon")
                .Text(
                    "{Flying}{EOL}Sacrifice another forward: Butcher of the Horde gains your choice of brave, lifelink, or haste until end of turn."
                )
                .Power(5)
                .Toughness(4)
                .SimpleAbilities(Static.Flying)
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "Sacrifice another forward: Butcher of the Horde gains brave until end of turn.";
                    p.Cost = new Sacrifice();
                    p.Effect = () => new OwnerGainsBraveLifelinkOrHaste();

                    p.TargetSelector.AddCost(
                        trg =>
                            trg.Is.Forward(ControlledBy.SpellOwner, canTargetSelf: false)
                                .On.Battlefield(),
                        trg => trg.Message = "Select a forward to sacrifice."
                    );

                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectOrCostRankBy(c => c.Score) { TargetLimit = 1 });
                });
        }
    }
}
