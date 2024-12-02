namespace CrystalArena.Effects
{
  using System;
  using System.Collections.Generic;
  using AI;
  using Decisions;
  using Events;
  using Infrastructure;
  using System.Linq;

  public class ChooseInBreakZonePutToZone : Effect, IProcessDecisionResults<ChosenCards>,
    IChooseDecisionResults<List<Card>, ChosenCards>, ICardValidator
  {
    private readonly EffectAction<Card> _afterPutToZone;
    private readonly int _maxCount;
    private readonly int _minCount;
    private readonly DynParam<Player> _player;
    private readonly bool _revealCards;
    private readonly CardOrder _rankingAlgorithm;

    private readonly string _text;
    private readonly CardSelector _validator;
    private readonly Zone _zone;

    private ChooseInBreakZonePutToZone() { }

    public ChooseInBreakZonePutToZone(Zone zone, EffectAction<Card> afterPutToZone = null,
      int maxCount = 1, int minCount = 0, CardSelector validator = null,
      string text = null, bool revealCards = true, DynParam<Player> player = null,
      CardOrder rankingAlgorithm = null)
    {
      _validator = validator ?? delegate   { return true; };
      _player = player ?? new DynParam<Player>((e, g) => e.Controller, EvaluateAt.OnResolve);
      _text = text ?? "Search your library for a card.";
      _zone = zone;
      _afterPutToZone = afterPutToZone ?? delegate { };
      _revealCards = revealCards;
      _rankingAlgorithm = rankingAlgorithm ?? ((c, ctx) => -c.Score);
      _maxCount = maxCount;
      _minCount = minCount;

      RegisterDynamicParameters(_player);
    }

    public bool IsValidCard(Card card)
    {
      return _validator(card, Ctx);
    }

    public ChosenCards ChooseResult(List<Card> candidates)
    {
      return CardPicker
        .ChooseBestCards(
          controller: _player.Value,
          candidates: candidates,
          count: _maxCount,
          aurasNeedTarget: false,
          rankingAlgorithm: c => _rankingAlgorithm(c, Ctx));
    }

    public void ProcessResults(ChosenCards results)
    {
      var i = 0;

      while (i < results.Count)
      {
        var card = results[i++];

        PutToZone(card);
      }
    }

    private void PutToZone(Card card)
    {
      switch (_zone)
      {
        case (Zone.Hand):
          {
            card.PutToHandFrom(Zone.MainDeck);
            break;
          }
        case (Zone.Battlefield):
          {
            card.PutToBattlefield();
            break;
          }
        case (Zone.BreakZone):
          {
            card.PutToBreakZone();
            break;
          }
        case (Zone.RemovedFromPlay):
          {
            card.RemoveFromPlay(this);
            break;
          }
        case (Zone.MainDeck):
          {
            Controller.ShuffleMainDeck();
            card.PutOnTopOfMainDeck();
            break;
          }
        default:
          {
            Asrt.Fail(String.Format("Zone not supported: {0}.", _zone));
            break;
          }
      }

      _afterPutToZone(card, Ctx);
    }

    protected override void ResolveEffect()
    {    

      Enqueue(new SelectCards(
        _player.Value,
        p =>
        {
          p.MinCount = _minCount;
          p.MaxCount = _maxCount;
          p.Validator = this;
          p.Zone = Zone.BreakZone;
          p.Text = _text;
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.OwningCard = Source.OwningCard;
          p.AurasNeedTarget = false;
        }));
    }
    
  }
}