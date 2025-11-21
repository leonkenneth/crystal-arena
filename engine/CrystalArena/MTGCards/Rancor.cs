namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class Rancor : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Rancor")
                .ManaCost("{G}")
                .Type("Monster - Aura")
                .Text(
                    "Enchanted forward gets +2/+0 and has trample.{EOL}When Rancor is put into a breakZone from the battlefield, return Rancor to its owner's hand."
                )
                .FlavorText("Hatred outlives the hateful.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(
                            () => new AddPowerAndToughness(2, 0),
                            () => new AddSimpleAbility(Static.Trample)
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Rancor is put into a breakZone from the battlefield, return Rancor to its owner's hand.";
                    p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
                    p.Effect = () => new ReturnToHand(returnOwningCard: true);
                });
        }
    }
}
