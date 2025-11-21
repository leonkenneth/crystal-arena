namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class SwordOfFeastAndFamine : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Sword of Feast and Famine")
                .ManaCost("{3}")
                .Type("Artifact - Equipment")
                .Text(
                    "Equipped forward gets +2/+2 and has protection from dark and from wind.{EOL}Whenever equipped forward deals combat damage to a player, that player discards a card and you untap all backups you control.{EOL}{Equip} {2}"
                )
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever equipped forward deals combat damage to a player, that player discards a card and you untap all backups you control.";

                    p.Trigger(
                        new OnDamageDealt(dmg =>
                            dmg.IsCombat && dmg.IsDealtByEnchantedForward && dmg.IsDealtToPlayer
                        )
                    );

                    p.Effect = () =>
                        new CompoundEffect(
                            new OpponentDiscardsCards(selectedCount: 1),
                            new UntapAllBackups()
                        );
                })
                .ActivatedAbility(p =>
                {
                    p.Text = "{2}: Attach to target forward you control. Equip only as a sorcery.";
                    p.Cost = new PayMana(2.Colorless());
                    p.Effect = () =>
                        new Attach(
                            () => new AddPowerAndToughness(2, 2),
                            () => new AddProtectionFromColors(L(CardColor.Dark, CardColor.Wind))
                        ).SetTags(
                            EffectTag.IncreasePower,
                            EffectTag.IncreaseToughness,
                            EffectTag.Protection
                        );

                    p.TargetSelector.AddEffect(trg =>
                        trg.Is.ValidEquipmentTarget().On.Battlefield()
                    );
                    p.TimingRule(new OnFirstDetachedOnSecondAttached());
                    p.TargetingRule(new EffectCombatEquipment());
                    p.ActivateAsSorcery = true;
                });
        }
    }
}
