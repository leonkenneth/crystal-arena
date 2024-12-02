namespace CrystalArena.CardsMainDeck
{
  using System.Collections.Generic;
  using CrystalArena.Effects;
  using CrystalArena.AI.TimingRules;
  using CrystalArena.Modifiers;
  using CrystalArena.Triggers;

  public class Retaliation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Retaliation")
        .ManaCost("{2}{G}")
        .Type("Monster")
        .Text(
          "Forwards you control have 'Whenever this forward becomes blocked by a forward, this forward gets +1/+1 until end of turn.'")
        .FlavorText("A foul, metallic stench clogged Urza's senses. It was then he knew his brother was no more.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () =>
              {
                var tp = new TriggeredAbility.Parameters
                  {
                    Text =
                      "Whenever this forward becomes blocked by a forward, this forward gets +1/+1 until end of turn.",
                    Effect = () => new ApplyModifiersToSelf(
                      () => new AddPowerAndToughness(1, 1) {UntilEot = true}),                    
                  };
                
                tp.Trigger(new WhenThisBecomesBlocked(triggerForEveryBlocker: true));                

                return new AddTriggeredAbility(new TriggeredAbility(tp));
              };

            p.Selector = (card, ctx) => card.Controller == ctx.You && card.Is().Forward;
          });
    }
  }
}