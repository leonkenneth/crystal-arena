namespace CrystalArena.UserInterface.MainDeckFilter
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Caliburn.Micro;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    private static readonly CardsSearch _cardsSearch = new CardsSearch();
    private readonly Dictionary<string, CardInfo> _cards;
    private readonly Func<CardInfo, int> _orderBy;
    private readonly BindableCollection<Result> _results = new BindableCollection<Result>();
    private readonly Func<CardInfo, object> _transformResult;
    
    public override object ToJson()
    {
      return new
      {
        Type = "MainDeckFilter",
      };
    }

    public ViewModel(IEnumerable<CardInfo> cards, Func<CardInfo, object> transformResult, Func<CardInfo, int> orderBy)
    {
      _orderBy = orderBy;
      _transformResult = transformResult ?? (x => x);
      _cards = cards.ToDictionary(x => x.Name, x => x);

      Light = Water = Dark = Fire = Wind = true;
    }

    [Updates("FilteredResult")]
    public virtual string Text { get; set; }

    [Updates("FilteredResult")]
    public virtual bool Light { get; set; }

    [Updates("FilteredResult")]
    public virtual bool Water { get; set; }

    [Updates("FilteredResult")]
    public virtual bool Dark { get; set; }

    [Updates("FilteredResult")]
    public virtual bool Fire { get; set; }

    [Updates("FilteredResult")]
    public virtual bool Wind { get; set; }

    public IEnumerable<Result> FilteredResult
    {
      get
      {
        UpdateView();
        return _results;
      }
    }

    public override void Initialize()
    {
      Task.Factory.StartNew(() =>
        {
          var cards = _cardsSearch.Query().Where(x => _cards.ContainsKey(x.Name));

          foreach (var card in cards.OrderBy(x => _orderBy(_cards[x.Name])).ThenBy(x => x.Name))
          {
            if (_cards.ContainsKey(card.Name))
            {
              var info = _cards[card.Name];
              var result = Bindable.Create<Result>();
              result.View = _transformResult(info);
              result.CardName = card.Name;
              result.IsVisible = true;
              _results.Add(result);
            }
          }
        });
    }


    private void UpdateView()
    {
      var results = new HashSet<string>();

      Task.Factory.StartNew(() =>
        {
          var cards = _cardsSearch
            .Query(Text)
            .Where(x => _cards.ContainsKey(x.Name));

          foreach (var card in cards)
          {
            if (
              (Light && card.HasColor(CardColor.Light)) ||
                (Water && card.HasColor(CardColor.Water)) ||
                (Dark && card.HasColor(CardColor.Dark)) ||
                (Fire && card.HasColor(CardColor.Fire)) ||
                (Wind && card.HasColor(CardColor.Wind)) ||
                (card.HasColor(CardColor.Colorless) || card.ManaCost == null)
              )
            {
              results.Add(card.Name);
            }
          }
        })
        .ContinueWith(tsk =>
          {
            foreach (var result in _results)
            {
              if (results.Contains(result.CardName))
              {
                if (!result.IsVisible)
                  result.IsVisible = true;
              }
              else if (result.IsVisible)
              {
                result.IsVisible = false;
              }
            }
          }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<CardInfo> cards, Func<CardInfo, object> transformResult, Func<CardInfo, int> orderBy);
    }

    public class Result
    {
      public object View { get; set; }
      public string CardName { get; set; }
      public virtual bool IsVisible { get; set; }
    }
  }
}