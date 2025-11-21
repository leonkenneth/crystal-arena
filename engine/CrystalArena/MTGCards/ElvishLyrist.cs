namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.AI.TargetingRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class ElvishLyrist : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Elvish Lyrist")
                .ManaCost("{G}")
                .Type("Forward Elf")
                .Text("{G},{T}, Sacrifice Elvish Lyrist: Destroy target monster.")
                .FlavorText(
                    "Bring the spear of ancient briar;{EOL}Bring the torch to light the pyre.{EOL}Bring the one who trod our ground;{EOL}Bring the spade to dig his mound."
                )
                .Power(1)
                .Toughness(1)
                .ActivatedAbility(p =>
                {
                    p.Text = "{G},{T}, Sacrifice Elvish Lyrist: Destroy target monster.";
                    p.Cost = new AggregateCost(new PayMana(Mana.Wind), new Tap(), new Sacrifice());
                    p.Effect = () => new DestroyTargetPermanents();
                    p.TargetSelector.AddEffect(trg => trg.Is.Monster().On.Battlefield());

                    p.TargetingRule(new EffectDestroy());
                    p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Destroy));
                });
        }
    }
}
