namespace CrystalArena.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Decisions;

  internal class RevealTopCardsPutOneInHandOthersIntoBreakZone : Effect,
    IProcessDecisionResults<ChosenCards>, IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly int _cardsToReveal;
    private readonly Func<Card, bool> _selector;

    private RevealTopCardsPutOneInHandOthersIntoBreakZone() {}

    public RevealTopCardsPutOneInHandOthersIntoBreakZone(int cardsToReveal, Func<Card, bool> selector)
    {
      _cardsToReveal = cardsToReveal;
      _selector = selector;
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return CardPicker
        .ChooseBestCards(
          controller: Controller,
          candidates: candidates,
          count: 1,
          aurasNeedTarget: false);
    }

    public void ProcessResults(ChosenCards results)
    {
      if (results.Count > 0)
      {
        results[0].PutToHand();
      }

      var notPicked = Controller.MainDeck
        .Take(_cardsToReveal - results.Count)
        .ToList();

      foreach (var card in notPicked)
      {
        card.PutToBreakZone();
      }      
    }

    protected override void ResolveEffect()
    {
      var cards = Controller.MainDeck
        .Take(_cardsToReveal)
        .ToList();

      foreach (var card in cards)
      {
        card.Reveal();
      }

      Enqueue(new SelectCards(
        Controller,
        p =>
          {
            p.MinCount = 0;
            p.MaxCount = 1;
            p.SetValidator(_selector);
            p.Zone = Zone.MainDeck;
            p.Text = "Select a card to put in hand.";
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
            p.OwningCard = Source.OwningCard;
            p.AurasNeedTarget = false;
          }));
    }
  }
}