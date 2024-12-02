namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Triggers;

  public class EngineeredPlague : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Engineered Plague")
        .ManaCost("{2}{B}")
        .Type("Monster")
        .Text(
          "As Engineered Plague enters the battlefield, choose a forward type.{EOL}All forwards of the chosen type get -1/-1.")
        .FlavorText("The admixture of bitterwort in the viral brew has produced most favorable results.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new ForwardsOfChosenTypeGainPT(-1, -1);
            p.UsesStack = false;
          });
    }
  }
}