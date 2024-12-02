using System;
using CrystalArena.AI.CostRules;

namespace CrystalArena.UserInterface.CardOrder
{
  public class OrderedObject
  {
    private string _oid;

    public OrderedObject(Dialogs viewModels, object cardOrEffect)
    {
      ViewModels = viewModels;
      Object = cardOrEffect;
    }

    public Dialogs ViewModels { get; set; }

    public object Object { get; private set; }
    public virtual int? Order { get; set; }

    public Card Card
    {
      get
      {
        var card = Object as Card;
        var effect = Object as Effect;
        var ability = Object as Ability;
        
        if (card != null) return card;
        if (effect != null) return effect.Card();
        if (ability != null) return ability.SourceCard;

        throw new ArgumentException("Unable to determine card from object.");
      }
    }

    public object ToJson()
    {
      return new
      {
        Order,
        Card = ViewModels.Card.Create(Card).ToJson(),
        Id
      };
    }

    public string Id
    {
      get
      {
        var card = Object as Card;
        var effect = Object as Effect;
        var ability = Object as Ability;
        
        if (card != null) return "Card/" + card.Id;
        if (effect != null) return "Effect/" + effect.Id;
        if (ability != null) return "Ability/" + ability.SourceCard.Id;

        throw new ArgumentException("Unable to determine id from object.");

      }
    }
  }
}