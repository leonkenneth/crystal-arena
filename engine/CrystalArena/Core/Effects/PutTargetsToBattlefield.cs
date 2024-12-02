namespace CrystalArena.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;

  public class PutTargetsToBattlefield : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>
  {
    private readonly bool _mustSacForwardOnResolve;
    private readonly bool _tapped;

    private PutTargetsToBattlefield() {}

    public PutTargetsToBattlefield(bool mustSacForwardOnResolve = false, bool tapped = false)
    {
      _mustSacForwardOnResolve = mustSacForwardOnResolve;
      _tapped = tapped;
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return candidates
        .OrderBy(x => x.Score)
        .Take(1)
        .ToList();
    }

    public void ProcessResults(ChosenCards results)
    {
      foreach (var card in results)
      {
        card.Sacrifice();
      }

      PutValidTargetsToBattlefield();
    }

    protected override void ResolveEffect()
    {
      if (_mustSacForwardOnResolve)
      {
        if (Controller.Battlefield.Forwards.None())
          return;

        SacForwardAndPutValidTargetsToBattlefield();
        return;
      }

      PutValidTargetsToBattlefield();
    }

    private void SacForwardAndPutValidTargetsToBattlefield()
    {
      Enqueue(new SelectCards(Controller, p =>
        {
          p.MinCount = 1;
          p.MaxCount = 1;
          p.SetValidator(card => card.Is().Forward);
          p.Zone = Zone.Battlefield;
          p.Text = "Select a forward to sacrifice";
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.OwningCard = Source.OwningCard;
        }));
    }

    private void PutValidTargetsToBattlefield()
    {
      foreach (var target in ValidEffectTargets)
      {
        var card = target.Card();

        Controller.PutCardToBattlefield(card);

        if (_tapped)
        {
          card.Tap();
        }
      }
    }
  }
}