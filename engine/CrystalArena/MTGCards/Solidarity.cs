namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class Solidarity : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Solidarity")
        .ManaCost("{3}{W}")
        .Type("Summon")
        .Text("Forwards you control get +0/+5 until end of turn.")
        .FlavorText("We must all hang together, or assuredly we shall all hang separately.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (c, ctx) => c.Controller == ctx.You && c.Is().Forward,
              modifier: () => new AddPowerAndToughness(0, 5) {UntilEot = true});

            p.TimingRule(new AfterOpponentDeclaresAttackers());
          });
    }
  }
}