namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.RepetitionRules;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;
    using ReturnToHand = Effects.ReturnToHand;

    public class FieryMantle : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Fiery Mantle")
                .ManaCost("{1}{R}")
                .Type("Monster - Aura")
                .Text(
                    "Enchant forward{EOL}{R}: Enchanted forward gets +1/+0 until end of turn.{EOL}When Fiery Mantle is put into a breakZone from the battlefield, return Fiery Mantle to its owner's hand."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new Attach(() =>
                        {
                            var ap = new ActivatedAbilityParameters
                            {
                                Text = "{R}: Enchanted forward gets +1/+0 until end of turn.",
                                Cost = new PayMana(Mana.Fire, supportsRepetitions: true),
                                Effect = () =>
                                    new ApplyModifiersToSelf(() =>
                                        new AddPowerAndToughness(1, 0) { UntilEot = true }
                                    ),
                            };

                            ap.TimingRule(new PumpOwningCardTimingRule(1, 0));
                            ap.RepetitionRule(new RepeatMaxTimes());

                            return new AddActivatedAbility(new ActivatedAbility(ap));
                        });

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatMonster());
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Fiery Mantle is put into a breakZone from the battlefield, return Fiery Mantle to its owner's hand.";
                    p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));
                    p.Effect = () => new ReturnToHand(returnOwningCard: true);
                });
        }
    }
}
