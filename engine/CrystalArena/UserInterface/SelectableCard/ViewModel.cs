using System.Linq;

namespace CrystalArena.UserInterface.SelectableCard
{
  using System;
  using Infrastructure;
  using Messages;

  public class ViewModel : CardViewModel, IReceive<TargetSelected>, IReceive<TargetUnselected>,
    IReceive<UiInteractionChanged>
  {
    private Action _select = delegate { };

    public ViewModel(Card card) : base(card) {}
    
    public override object ToJson()
    {
      return new
      {
        Type = "SelectableCard",
        // Card
        CardId = Card.Id,
        Name,
        HasXInCost,
        ManaCost = ManaCost?.ToString(),
        Illustration,
        Text = Text.ToString(),
        CharacterCount,
        Power,
        Toughness,
        BasePower,
        BaseToughness,
        IsVisibleInUi,
        Colors = Colors.Select(x => x.ToString()),
        Counters,
        SimpleAbilities,
        Level,
        Damage,
        IsTapped,
        HasSummoningSickness,
        Set,
        Rarity,
        Loyality,
        Serial,
        // End card,
        IsSelected,
        Oid = base.ToJsonWithOid(),
      };
    }

    public virtual bool IsSelected { get; protected set; }

    public void Receive(TargetSelected message)
    {
      if (message.Target == Card)
      {
        IsSelected = true;
      }
    }

    public void Receive(TargetUnselected message)
    {
      if (message.Target == Card)
      {
        IsSelected = false;
      }
    }

    public void Receive(UiInteractionChanged message)
    {
      switch (message.State)
      {
        case (InteractionState.SelectTarget):
          {
            _select = ChangeSelection;
            break;
          }
        default:
          _select = delegate { };
          break;
      }

      IsSelected = false;
    }

    private void ChangeSelection()
    {
      Ui.Publisher.Publish(
        new SelectionChanged {Selection = Card});
    }

    public void Select()
    {
      _select();
    }

    public void ChangePlayersInterest()
    {
      ChangePlayersInterest(this);
    }

    public override void ReceiveMessageType(string type, string message)
    {
      if (type == "Select")
      {
        Select();
        return;
      }
      
      throw new ArgumentException("Unknown message type: " + type);
    }

    public interface IFactory
    {
      ViewModel Create(Card card);
      void Destroy(ViewModel viewModel);
    }
  }
}