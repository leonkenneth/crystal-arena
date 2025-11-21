namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class IronWill : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Iron Will")
                .ManaCost("{W}")
                .Type("Summon")
                .Text(
                    "Target forward gets +0/+4 until end of turn.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)"
                )
                .Cycling("{2}")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddPowerAndToughness(0, 4) { UntilEot = true }
                        ).SetTags(EffectTag.IncreaseToughness);

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());
                    p.TimingRule(new PumpTargetCardTimingRule());
                    p.TargetingRule(new EffectPumpSummon(0, 4));
                });
        }
    }
}
