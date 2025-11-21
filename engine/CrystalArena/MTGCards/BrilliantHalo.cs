namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;
    using Triggers;

    public class BrilliantHalo : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Brilliant Halo")
                .ManaCost("{1}{W}")
                .Type("Monster Aura")
                .Text(
                    "Enchanted forward{EOL}Enchanted forward gets +1/+2.{EOL}When Brilliant Halo is put into a breakZone from the battlefield, return Brilliant Halo to its owner's hand."
                )
                .Cast(p =>
                {
                    p.Effect = () => new Attach(() => new AddPowerAndToughness(1, 2));
                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Brilliant Halo is put into a breakZone from the battlefield, return Brilliant Halo to its owner's hand.";
                    p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
                    p.Effect = () => new ReturnToHand(returnOwningCard: true);
                });
        }
    }
}
