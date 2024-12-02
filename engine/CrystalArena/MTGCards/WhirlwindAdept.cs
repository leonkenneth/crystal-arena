namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;

  public class WhirlwindAdept : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Whirlwind Adept")
        .ManaCost("{4}{U}")
        .Type("Forward — Djinn Monk")
        .Text("{Hexproof} {I}(This forward can't be the target of spells or abilities your opponents control.){/I}{EOL}{Prowess} {I}(Whenever you cast a nonforward spell, this forward gets +1/+1 until end of turn.){/I}")
        .Power(4)
        .Toughness(2)
        .Prowess()
        .SimpleAbilities(Static.Hexproof);
    }
  }
}
