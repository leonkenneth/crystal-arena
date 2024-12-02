namespace CrystalArena.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Castle.Core.Internal;
  using Decisions;

  public class PutSelectedCardsIntoBreakZoneOthersOnTop : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>, IProcessDecisionResults<Ordering>, IChooseDecisionResults<List<Card>, Ordering>
  {
    private readonly int _count;
    private readonly int? _countBreakZone;

    private PutSelectedCardsIntoBreakZoneOthersOnTop() {}

    public PutSelectedCardsIntoBreakZoneOthersOnTop(
      int count, 
      int? countBreakZone = null)
    {
      _count = count;
      _countBreakZone = countBreakZone;
    }

    protected override void ResolveEffect()
    {
      var cards = Controller.MainDeck
        .Take(_count)
        .ToList();

      foreach (var card in cards)
      {
        card.Peek();
      }

      Enqueue(new SelectCards(Controller,
        p =>
        {
          p.SetValidator(c => cards.Contains(c));
          p.Zone = Zone.MainDeck;
          p.MinCount = _countBreakZone ?? 0;
          p.MaxCount = _countBreakZone;
          p.Text = "Select cards to put into breakZone.";
          p.OwningCard = Source.OwningCard;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
        }
        ));
    }

    ChosenCards IChooseDecisionResults<List<Card>, ChosenCards>.ChooseResult(List<Card> candidates)
    {
      var backupsInPlay = Controller.Battlefield.Backups.Count();

      var needsBackups = !Controller.Hand.Backups.Any() &&
        backupsInPlay <= 5;

      // Select cards to be put into breakZone
      return candidates
        .Where(x =>
        {
          if (x.Is().Backup)
          {
            return !needsBackups;
          }

          // If card cannot be played return true (it will be put into breakZone)
          return x.ConvertedCost > backupsInPlay;
        })
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.PutToBreakZone();
      }

      if (results.Count == _count)
        return;

      // Put the rest cards back on top library in any order
      var cards = Controller.MainDeck
        .Take(_count - results.Count)
        .ToList();

      Enqueue(new OrderCards(
        Controller,
        p =>
        {
          p.Cards = cards;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.Title = "Order cards from top to bottom";
        }));
    }

    Ordering IChooseDecisionResults<List<Card>, Ordering>.ChooseResult(List<Card> candidates)
    {
      return QuickDecisions.OrderTopCards(candidates, Controller);
    }

    public void ProcessResults(Ordering result)
    {
      Controller.ReorderTopCardsOfMainDeck(result.Indices);
    }
  }
}
