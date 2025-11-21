namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;
    using Modifiers;
    using Triggers;

    public class HotSoup : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Hot Soup")
                .ManaCost("{1}")
                .Type("Artifact — Equipment")
                .Text(
                    "Equipped forward can't be blocked.{EOL}Whenever equipped forward is dealt damage, destroy it.{EOL}Equip {3}({3}: Attach to target forward you control. Equip only as a sorcery.)"
                )
                .FlavorText("\"Comin' through!\"")
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "Equip {3} ({3}: Attach to target forward you control. Equip only as a sorcery.)";

                    p.Cost = new PayMana(3.Colorless());

                    p.Effect = () => new Attach(() => new AddSimpleAbility(Static.Unblockable));

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.ValidEquipmentTarget().On.Battlefield()
                    );

                    p.TimingRule(new OnFirstDetachedOnSecondAttached());
                    p.TargetingRule(new EffectCombatEquipment());

                    p.ActivateAsSorcery = true;
                })
                .TriggeredAbility(p =>
                {
                    p.Text = "Whenever equipped forward is dealt damage, destroy it.";

                    p.Trigger(new OnDamageDealt(dmg => dmg.IsDealtToEnchantedForward));
                    p.Effect = () => new DestroyPermanent(P(e => e.Source.OwningCard.AttachedTo));

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
