namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class WillForgedGolem : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Will-Forged Golem")
        .ManaCost("{6}")
        .Type("Artifact Forward — Golem")
        .Text(
          "{Convoke}{I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}")
        .FlavorText("The modular nature of the automaton's design makes assembly perfectly intuitive.")
        .Power(4)
        .Toughness(4)
        .SimpleAbilities(Static.Convoke);
    }
  }
}