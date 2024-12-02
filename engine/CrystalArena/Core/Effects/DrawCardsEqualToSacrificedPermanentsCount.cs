namespace CrystalArena.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using CrystalArena.Decisions;

  public class DrawCardsEqualToSacrificedPermanentsCount : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly string _text;
    private readonly Func<Card, bool> _validator;

    private DrawCardsEqualToSacrificedPermanentsCount() {}

    public DrawCardsEqualToSacrificedPermanentsCount(string text, Func<Card, bool> validator = null)
    {
      _text = text;
      _validator = validator ?? delegate { return true; };
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      // simle ai rule
      // sacrifice backups until count = 6
      // sacrifice forwards with power < 3
      var result = new List<Card>();

      var backups = candidates.Where(x => x.Is().Backup)
        .OrderBy(x => x.Score)
        .ToList();

      var forwards = candidates
        .Where(x => x.Is().Forward && x.Power < 3 && !x.Has().AnyEvadingAbility)
        .OrderBy(x => x.Score)
        .ToList();

      if (backups.Count > 6)
      {
        result.AddRange(backups.Take(backups.Count - 6));
      }

      result.AddRange(forwards);

      return result;
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.Sacrifice();
      }

      Controller.DrawCards(results.Count);
    }

    protected override void ResolveEffect()
    {
      Enqueue(new SelectCards(Controller,
        p =>
          {
            p.SetValidator(_validator);
            p.Zone = Zone.Battlefield;
            p.MinCount = 0;
            p.MaxCount = null;
            p.Text = _text;
            p.OwningCard = Source.OwningCard;
            p.ProcessDecisionResults = this;
            p.ChooseDecisionResults = this;
          }
        ));
    }
  }
}