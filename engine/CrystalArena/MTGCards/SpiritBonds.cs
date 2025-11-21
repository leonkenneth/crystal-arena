namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;
    using Modifiers;
    using Triggers;

    public class SpiritBonds : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Spirit Bonds")
                .ManaCost("{1}{W}")
                .Type("Monster")
                .Text(
                    "Whenever a nontoken forward enters the battlefield under your control, you may pay {W}. If you do, put a 1/1 light Spirit forward token with flying onto the battlefield.{EOL}{1}{W}, Sacrifice a Spirit: Target non-Spirit forward gains indestructible until end of turn. {I}(Damage and effects that say \"destroy\" don't destroy it.){/I}"
                )
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever a nontoken forward enters the battlefield under your control, you may pay {W}. If you do, put a 1/1 light Spirit forward token with flying onto the battlefield.";
                    p.Trigger(
                        new OnZoneChanged(
                            to: Zone.Battlefield,
                            selector: (c, ctx) =>
                                ctx.You == c.Controller && c.Is().Forward && !c.Is().Token
                        )
                    );

                    p.Effect = () =>
                        new PayManaThen(
                            Mana.Light,
                            new CreateTokens(
                                count: 1,
                                token: Card.Named("Spirit")
                                    .Power(1)
                                    .Toughness(1)
                                    .Type("Token Forward - Spirit")
                                    .Text("{Flying}")
                                    .Colors(CardColor.Light)
                                    .SimpleAbilities(Static.Flying)
                            )
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{1}{W}, Sacrifice a Spirit: Target non-Spirit forward gains indestructible until end of turn.";

                    p.Cost = new AggregateCost(new PayMana("{1}{W}".Parse()), new Sacrifice());

                    p.TargetSelector.AddCost(
                        trg =>
                            trg.Is.Card(c => c.Is("Spirit"), ControlledBy.SpellOwner)
                                .On.Battlefield(),
                        trg => trg.Message = "Select a Spirit to sacrifice."
                    );

                    p.TargetSelector.AddEffect(
                        trg => trg.Is.Card(c => c.Is().Forward && !c.Is("Spirit")).On.Battlefield(),
                        trg => trg.Message = "Select a non-Spirit forward to gain indestructible."
                    );

                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddSimpleAbility(Static.Indestructible) { UntilEot = true }
                        ).SetTags(EffectTag.Indestructible);

                    p.TargetingRule(new CostSacrificeEffectGiveIndestructible());
                });
        }
    }
}
