namespace CrystalArena.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public class Scry : Effect, IProcessDecisionResults<Split>,
    IChooseDecisionResults<List<Card>, Split>
  {
    private readonly int _count;

    private Scry() {}

    public Scry(int count)
    {
      _count = count;
    }

    public Split ChooseResult(List<Card> candidates)
    {
      var backupCount = Controller.Battlefield.Backups.Count() +
        Controller.Hand.Backups.Count();

      var needsBackups = backupCount < 6;

      var top = new List<Card>();
      var bottom = new List<Card>();

      foreach (var candidate in candidates)
      {
        if (candidate.Is().Backup)
        {
          if (needsBackups)
          {
            top.Add(candidate);
          }
          else
          {
            bottom.Add(candidate);
          }
        }

        else if (candidate.ConvertedCost <= backupCount)
        {
          top.Add(candidate);
        }
        else
        {
          bottom.Add(candidate);
        }
      }

      // Put backups on top, then order by descending score.
      // Ordering of the cards which are put on the 
      // bottom is not necessary.
      top = top
        .OrderBy(x => x.Is().Backup ? int.MinValue : -x.Score)
        .ToList();

      return new Split(new[]
        {
          top,
          bottom
        });
    }

    public void ProcessResults(Split result)
    {
      IEnumerable<Card> top = result.Groups[0];
      var bottom = result.Groups[1];

      foreach (var card in bottom)
      {
        Controller.PutOnBottomOfMainDeck(card);        
      }

      foreach (var card in top.Reverse())
      {
        Controller.PutCardOnTopOfMainDeck(card);
      }
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

      Enqueue(new SplitCards(
        Controller,
        p =>
          {
            p.Cards = cards;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
            p.SplitText = "Select only cards which will be put on the top of library.";
            p.PositiveText = "Order cards to put on the top.";
            p.NegativeText = "Order cards to put on the bottom.";
          }));
    }
  }
}