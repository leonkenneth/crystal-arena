namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI;
    using CrystalArena.Effects;
    using CrystalArena.Triggers;

    public class Gamekeeper : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Gamekeeper")
                .ManaCost("{3}{G}")
                .Type("Forward Elf")
                .Text(
                    "When Gamekeeper dies, you may exile it. If you do, reveal cards from the top of your library until you reveal a forward card. Put that card onto the battlefield and put all other cards revealed this way into your breakZone."
                )
                .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
                .Power(2)
                .Toughness(2)
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When Gamekeeper dies, you may exile it. If you do, reveal cards from the top of your library until you reveal a forward card. Put that card onto the battlefield and put all other cards revealed this way into your breakZone.";
                    p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));

                    p.Effect = () =>
                        new CompoundEffect(
                            new RemoveFromPlayOwner(),
                            new PutFirstCardInPlayPutOtherCardsToZone(
                                Zone.BreakZone,
                                filter: c => c.Is().Forward
                            )
                        );
                });
        }
    }
}
