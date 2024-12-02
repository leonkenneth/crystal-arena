namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;

  public class HeadlongRush : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Headlong Rush")
        .ManaCost("{1}{R}")
        .Type("Summon")
        .Text("Attacking forwards gain first strike until end of turn.")
        .FlavorText(
          "A backupslide of goblins poured towards the defenders—tumbling, rolling, and bouncing their way down the steep hillside.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (c, ctx) => c.IsAttacker,
              modifier: () => new AddSimpleAbility(Static.FirstStrike) {UntilEot = true});
            
            p.TimingRule(new OnYourTurn(Step.DeclareBlocker));            
          });
    }
  }
}