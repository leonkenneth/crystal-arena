namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class Vineweft : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Vineweft")
        .ManaCost("{G}")
        .Type("Monster — Aura")
        .Text("Enchant forward{EOL}Enchanted forward gets +1/+1.{EOL}{4}{G}: Return Vineweft from your breakZone to your hand.")
        .FlavorText("Fortified by the wilds.")
        .Cast(p =>
        {
          p.Effect = () => new Attach(() => new AddPowerAndToughness(1, 1)).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
          p.TargetSelector.AddEffect(trg => trg.Is.Forward().On.Battlefield());

          p.TargetingRule(new EffectCombatMonster());
          p.TimingRule(new OnFirstMain());
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{4}{G}: Return Vineweft from your breakZone to your hand.";

          p.Cost = new PayMana("{4}{G}".Parse());

          p.Effect = () => new Effects.ReturnToHand(returnOwningCard: true);
          p.ActivationZone = Zone.BreakZone;
          
          p.TimingRule(new OnEndOfOpponentsTurn());
        });
    }
  }
}
