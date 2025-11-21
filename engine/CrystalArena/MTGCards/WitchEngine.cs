namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;

    public class WitchEngine : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Witch Engine")
                .ManaCost("{5}{B}")
                .Type("Forward Horror")
                .Text(
                    "{Swampwalk}{EOL}{T}: Add {B}{B}{B}{B} to your mana pool. Target opponent gains control of Witch Engine. (Activate this ability only any time you could cast an summon.)"
                )
                .Power(4)
                .Toughness(4)
                .SimpleAbilities(Static.Swampwalk)
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{T}: Add {B}{B}{B}{B} to your mana pool. Target opponent gains control of Witch Engine. (Activate this ability only any time you could cast an summon.)";

                    p.Cost = new Tap();

                    p.Effect = () =>
                        new CompoundEffect(
                            new AddManaToPool("{B}{B}{B}{B}".Parse()),
                            new SwitchController()
                        );

                    p.TimingRule(new WhenYouNeedAdditionalMana(4));
                });
        }
    }
}
