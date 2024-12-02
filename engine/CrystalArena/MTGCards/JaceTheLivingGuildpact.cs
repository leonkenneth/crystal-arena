namespace CrystalArena.CardsMainDeck
{
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using CrystalArena.Decisions;
  using System.Collections.Generic;

  public class JaceTheLivingGuildpact : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jace, the Living Guildpact")
        .ManaCost("{2}{U}{U}")
        .Type("Planeswalker Jace")
        .Text("{+1}: Look at the top two cards of your library. Put one of them into your breakZone.{EOL}" +
        "{-3}: Return another target nonland permanent to its owner's hand.{EOL}" +
        "{-8}: Each player shuffles their hand and breakZone into their library. You draw seven cards.")
        .Loyality(5)
        .ActivatedAbility(p =>
        {
          p.Text = "{+1}: Look at the top two cards of your library. Put one of them into your breakZone.";
          p.Cost = new AddCountersCost(CounterType.Loyality, 1);
          p.Effect = () => new PutSelectedCardsIntoBreakZoneOthersOnTop(2, 1);
          p.TimingRule(new OnFirstMain());
          p.ActivateAsSorcery = true;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{-3}: Return another target nonland permanent to its owner's hand.";
          p.Cost = new RemoveCounters(CounterType.Loyality, 3);
          p.Effect = () => new Effects.ReturnToHand();
          p.TargetSelector.AddEffect(trg => trg.Card(c => !c.Is().Backup, canTargetSelf: false).On.Battlefield());
          p.TargetingRule(new EffectBounce());
          p.TimingRule(new OnFirstMain());
          p.ActivateAsSorcery = true;
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{-8}: Each player shuffles their hand and breakZone into their library. You draw seven cards.";
          p.Cost = new RemoveCounters(CounterType.Loyality, 8);
          p.Effect = () => new EachPlayerShufflesHandAndBreakZoneIntoMainDeckAndDrawsCards(7, onlyYouDraw: true);
          p.TimingRule(new OnFirstMain());
          p.ActivateAsSorcery = true;
        });
    }
  }
}