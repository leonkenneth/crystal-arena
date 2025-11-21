namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class BlessedReversal : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Blessed Reversal")
                .ManaCost("{1}{W}")
                .Type("Summon")
                .Text("You gain 3 life for each forward attacking you.")
                .FlavorText("Even an enemy is a valuable resource.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new ChangeLife(
                            P((e, g) => e.Controller.IsActive ? 0 : g.Combat.AttackerCount * 3),
                            P(e => e.Controller)
                        );

                    p.TimingRule(new AfterOpponentDeclaresAttackers());
                });
        }
    }
}
