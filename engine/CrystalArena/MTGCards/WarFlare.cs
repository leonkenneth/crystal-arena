namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class WarFlare : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("War Flare")
        .ManaCost("{2}{R}{W}")
        .Type("Summon")
        .Text("Forwards you control get +2/+1 until end of turn. Untap those forwards.")
        .FlavorText("\"No sun or fire can warm my blood quite like a war flare.\"{EOL}—Urut Barzeel, Mardu hordechief")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new UntapEachPermanent(c => c.Is().Forward, controlledBy: ControlledBy.SpellOwner),
            new ApplyModifiersToPermanents(
              (c, ctx) => c.Is().Forward && ctx.You == c.Controller,
                () => new AddPowerAndToughness(2, 1) { UntilEot = true }).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness));

          p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new AfterOpponentDeclaresBlockers()));
        });
    }
  }
}
