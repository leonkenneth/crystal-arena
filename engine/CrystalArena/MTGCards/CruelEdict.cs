namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class CruelEdict : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Cruel Edict")
                .ManaCost("{1}{B}")
                .Type("Sorcery")
                .Text("Target opponent sacrifices a forward.")
                .FlavorText("Choose your next words carefully. They will be your last.")
                .Cast(p =>
                {
                    p.Effect = () =>
                        new PlayerSacrificePermanents(
                            count: 1,
                            player: P(e => e.Controller.Opponent),
                            filter: c => c.Is().Forward,
                            text: "Sacrifice a forward."
                        );

                    p.TimingRule(new NonTargetRemovalTimingRule(1));
                });
        }
    }
}
