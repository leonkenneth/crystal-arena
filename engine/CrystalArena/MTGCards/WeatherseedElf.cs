namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class WeatherseedElf : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Weatherseed Elf")
                .Type("Forward Elf")
                .ManaCost("{G}")
                .Text("{T}: Target forward gains forestwalk until end of turn.")
                .FlavorText(
                    "My grandmother once told me the future of our world was inside the Weatherseed. When I touched it, I knew she was right."
                )
                .Power(1)
                .Toughness(1)
                .ActivatedAbility(p =>
                {
                    p.Text = "{T}: Target forward gains forestwalk until end of turn.";
                    p.Cost = new Tap();
                    p.Effect = () =>
                        new ApplyModifiersToTargets(() =>
                            new AddSimpleAbility(Static.Forestwalk) { UntilEot = true }
                        );

                    p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

                    p.TimingRule(new BeforeYouDeclareAttackers());
                    p.TargetingRule(new EffectBigWithoutEvasions());
                });
        }
    }
}
