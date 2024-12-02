namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using Costs;
  using Effects;
  using AI.TimingRules;

  public class BloodsoakedChampion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bloodsoaked Champion")
        .ManaCost("{B}")
        .Type("Forward — Human Warrior")
        .Text("Bloodsoaked Champion can't block.{EOL}{I}Raid{/I} — {1}{B}: Return Bloodsoaked Champion from your breakZone to the battlefield. Activate this ability only if you attacked with a forward this turn.")
        .FlavorText("\"Death is merely another foe the Mardu will overcome.\"")
        .Power(2)
        .Toughness(1)
        .ActivatedAbility(p =>
        {
          p.Text = "{1}{B}: Return Bloodsoaked Champion from your breakZone to the battlefield.";

          p.Cost = new PayMana("{1}{B}".Parse());
          p.Condition = (card, game) => game.Turn.Events.HasActivePlayerAttackedThisTurn;
          p.Effect = () => new PutOwnerToBattlefield(from: Zone.BreakZone);
          p.ActivationZone = Zone.BreakZone;

          p.TimingRule(new OnSecondMain());
        });
    }
  }
}
