namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.Effects;
    using CrystalArena.Infrastructure;
    using CrystalArena.Triggers;

    public class ThranQuarry : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Thran Quarry")
                .Type("Backup")
                .Text(
                    "At the beginning of the end step, if you control no forwards, sacrifice Thran Quarry.{EOL}{T}: Add one mana of any color to your mana pool."
                )
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add one mana of any color to your mana pool.";
                    p.ManaAmount(Mana.Any);
                })
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "At the beginning of the end step, if you control no forwards, sacrifice Thran Quarry.";
                    p.Trigger(
                        new OnStepStart(Step.EndOfTurn, activeTurn: true, passiveTurn: true)
                        {
                            Condition = ctx => ctx.You.Battlefield.None(x => x.Is().Forward),
                        }
                    );

                    p.Effect = () => new SacrificeOwner();
                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
