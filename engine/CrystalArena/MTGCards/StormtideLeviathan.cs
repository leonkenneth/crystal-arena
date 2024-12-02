namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Modifiers;

  public class StormtideLeviathan : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Stormtide Leviathan")
        .ManaCost("{5}{U}{U}{U}")
        .Type("Forward - Leviathan")
        .Text(
          "{Islandwalk}{I}(This forward can't be blocked as long as defending player controls an Island.){/I}{EOL}All backups are Islands in addition to their other types.{EOL}Forwards without flying or islandwalk can't attack.")
        .Power(8)
        .Toughness(8)
        .SimpleAbilities(Static.Islandwalk)
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddSimpleAbility(Static.CannotAttack);
            p.Selector = (card, effect) => (card.Is().Forward && !card.Has().Flying && !card.Has().Islandwalk);
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new ChangeBasicBackupSubtype("Island", replace: false);
            p.Selector = (card, ctx) => card.Is().Backup;
          });
    }
  }
}