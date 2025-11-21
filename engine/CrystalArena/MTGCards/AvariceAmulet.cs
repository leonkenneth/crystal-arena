namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;
    using Modifiers;
    using Triggers;

    public class AvariceAmulet : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Avarice Amulet")
                .ManaCost("{4}")
                .Type("Artifact — Equipment")
                .Text(
                    "Equipped forward gets +2/+0 and has brave and \"At the beginning of your upkeep, draw a card.\"{EOL}When equipped forward dies, target opponent gains control of Avarice Amulet.{EOL}Equip {2} ({2}: Attach to target forward you control. Equip only as a sorcery.)"
                )
                .FlavorText("")
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "Equip {2} ({2}: Attach to target forward you control. Equip only as a sorcery.)";

                    p.Cost = new PayMana(2.Colorless());

                    p.Effect = () =>
                        new Attach(
                            () => new AddPowerAndToughness(2, 0),
                            () => new AddSimpleAbility(Static.Brave),
                            () =>
                            {
                                var tp = new TriggeredAbility.Parameters();

                                tp.Text = "At the beginning of your upkeep, draw a card.";
                                tp.Trigger(new OnStepStart(Step.Upkeep));
                                tp.Effect = () => new DrawCards(1);

                                return new AddTriggeredAbility(new TriggeredAbility(tp));
                            }
                        );

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.ValidEquipmentTarget().On.Battlefield()
                    );

                    p.TimingRule(new OnFirstDetachedOnSecondAttached());
                    p.TargetingRule(new EffectCombatEquipment());

                    p.ActivateAsSorcery = true;
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When equipped forward dies, target opponent gains control of Avarice Amulet.";

                    p.Trigger(
                        new OnZoneChanged(
                            from: Zone.Battlefield,
                            to: Zone.BreakZone,
                            selector: (c, ctx) => c == ctx.OwningCard.AttachedTo
                        )
                    );

                    p.Effect = () => new SwitchController();
                });
        }
    }
}
