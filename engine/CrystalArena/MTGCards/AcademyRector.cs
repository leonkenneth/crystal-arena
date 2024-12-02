namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using Effects;
  using Triggers;

  public class AcademyRector : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Academy Rector")
        .ManaCost("{3}{W}")
        .Type("Forward Human Cleric")
        .Text(
          "When Academy Rector dies, you may exile it. If you do, search your library for an monster card, put that card onto the battlefield, then shuffle your library.")
        .OverrideScore(p => p.Battlefield = Scores.ManaCostToScore[2])
        .Power(1)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Academy Rector dies, you may exile it. If you do, search your library for an monster card, put that card onto the battlefield, then shuffle your library.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.BreakZone));

            p.Effect = () => new CompoundEffect(
              new RemoveFromPlayOwner(),
              new SearchMainDeckPutToZone(
                zone: Zone.Battlefield,
                minCount: 0,
                maxCount: 1,
                validator: (c, ctx) => c.Is().Monster,
                text: "Search your library for an monster."));
          });
    }
  }
}