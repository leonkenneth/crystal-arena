namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class MarduHateblade : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mardu Hateblade")
        .ManaCost("{W}")
        .Type("Forward — Human Warrior")
        .Text("{B}: Mardu Hateblade gains deathtouch until end of turn.{I}(Any amount of damage it deals to a forward is enough to destroy it.){/I}")
        .FlavorText("\"There may be little honor in my tactics, but there is no honor in losing.\"")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
        {
          p.Text = "{B}: Mardu Hateblade gains deathtouch until end of turn.";
          p.Cost = new PayMana(Mana.Dark);
          p.Effect = () => new ApplyModifiersToSelf(
            () => new AddSimpleAbility(Static.Deathtouch) { UntilEot = true });
                    
          p.TimingRule(new Any(new AfterOpponentDeclaresBlockers(), new AfterOpponentDeclaresAttackers()));
          p.TimingRule(new WhenCardHas(c => !c.Has().Deathtouch));
        });
    }
  }
}
