namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;

    public class LilianaVess : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Liliana Vess")
                .ManaCost("{3}{B}{B}")
                .Type("Planeswalker Liliana")
                .Text(
                    "{+1}: Target player discards a card.{EOL}"
                        + "{-2}: Search your library for a card, then shuffle your library and put that card on top of it.{EOL}"
                        + "{-8}: Put all forward cards from all breakZones onto the battlefield under your control.{EOL}"
                )
                .Loyality(5)
                .ActivatedAbility(p =>
                {
                    p.Text = "{+1}: Target player discards a card.";
                    p.Cost = new AddCountersCost(CounterType.Loyality, 1);
                    p.Effect = () => new Effects.DiscardCards(1);
                    p.TargetSelector.AddEffect(s => s.Is.Player());
                    p.TargetingRule(new EffectOpponent());
                    p.TimingRule(new OnFirstMain());
                    p.ActivateAsSorcery = true;
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{-2}: Search your library for a card, then shuffle your library and put that card on top of it.";
                    p.Cost = new RemoveCounters(CounterType.Loyality, 2);

                    p.Effect = () => new SearchMainDeckPutToZone(Zone.MainDeck, revealCards: false);
                    p.TimingRule(new OnFirstMain());
                    p.ActivateAsSorcery = true;
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{-8}: Put all forward cards from all breakZones onto the battlefield under your control.";
                    p.Cost = new RemoveCounters(CounterType.Loyality, 8);
                    p.Effect = () => new PutForwardsFromBreakZonesToYourBattlefield();
                    p.TimingRule(new OnFirstMain());
                    p.ActivateAsSorcery = true;
                });
        }
    }
}
