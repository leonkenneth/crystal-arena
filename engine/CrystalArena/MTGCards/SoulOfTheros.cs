namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class SoulOfTheros : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Soul of Theros")
        .ManaCost("{4}{W}{W}")
        .Type("Forward — Avatar")
        .Text("{Brave}{EOL}{4}{W}{W}: Forwards you control get +2/+2 and gain first strike and lifelink until end of turn.{EOL}{4}{W}{W}, RemoveFromPlay Soul of Theros from your breakZone: Forwards you control get +2/+2 and gain first strike and lifelink until end of turn.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Brave)
        .ActivatedAbility(p =>
        {
          p.Text = "{4}{W}{W}: Forwards you control get +2/+2 and gain first strike and lifelink until end of turn.";
          p.Cost = new PayMana("{4}{W}{W}".Parse());

          p.Effect = () => new ApplyModifiersToPermanents(
            selector: (c, ctx) => c.Is().Forward && ctx.You == c.Controller,            
            modifiers: L(
              () => new AddPowerAndToughness(2, 2){UntilEot = true},
              () => new AddSimpleAbility(Static.FirstStrike){UntilEot = true},
              () => new AddSimpleAbility(Static.Lifelink){UntilEot = true})
            ).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

          p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new BeforeYouDeclareAttackers()));
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{4}{W}{W}, RemoveFromPlay Soul of Theros from your breakZone: Forwards you control get +2/+2 and gain first strike and lifelink until end of turn.";
          p.Cost = new AggregateCost(
            new PayMana("{4}{W}{W}".Parse()),
            new RemoveFromPlayOwnerCost());

          p.Effect = () => new ApplyModifiersToPermanents(
            selector: (c, ctx) => c.Is().Forward && ctx.You == c.Controller,            
            modifiers: new CardModifierFactory[]
            {
              () => new AddPowerAndToughness(2, 2){UntilEot = true},
              () => new AddSimpleAbility(Static.FirstStrike){UntilEot = true},
              () => new AddSimpleAbility(Static.Lifelink){UntilEot = true}
            }).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

          p.ActivationZone = Zone.BreakZone;

          p.TimingRule(new Any(new AfterOpponentDeclaresAttackers(), new BeforeYouDeclareAttackers()));
        });
    }
  }
}
