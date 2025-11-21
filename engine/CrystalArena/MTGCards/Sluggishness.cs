namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class Sluggishness : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Sluggishness")
                .ManaCost("{1}{R}")
                .Type("Monster Aura")
                .Text(
                    "Enchanted forward can't block.{EOL}When Sluggishness is put into a breakZone from the battlefield, return Sluggishness to its owner's hand."
                )
                .FlavorText("Vark decided to lie down and think of a good excuse to quit working.")
                .Cast(p =>
                {
                    p.Effect = () => new Attach(() => new AddSimpleAbility(Static.CannotBlock));

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCannotBlockAttack(blockOnly: true));
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Sluggishness is put into a breakZone from the battlefield, return Sluggishness to its owner's hand.";
                    p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
                    p.Effect = () => new ReturnToHand(returnOwningCard: true);
                });
        }
    }
}
