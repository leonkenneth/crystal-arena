using Newtonsoft.Json;

namespace CrystalArena.UserInterface.SelectEffectChoice
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Effects;
  using Infrastructure;

  public class ViewModel : ViewModelBase
  {
    private readonly List<EffectChoiceViewModel> _choices;
    
    public override object ToJson()
    {
      
      return new
      {
        Type = "SelectEffectChoice",
        Message,
        Choices = _choices.Select(x => x.ToJson()),
        Oid = base.ToJsonWithOid()
      };
    }

    public ViewModel(IEnumerable<IEffectChoice> choices, string text)
    {
      _choices = choices.Select(x => new EffectChoiceViewModel(x)).ToList();

      Message = text
        .Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
        .Select(token =>
          {
            if (token.StartsWith("#"))
            {
              var index = int.Parse(token.Substring(1, 1));
              return _choices[index];
            }

            return (object) token;
          })
        .ToList();
    }

    public List<object> Message { get; private set; }
    public ChosenOptions ChosenOptions { get { return new ChosenOptions(_choices.Select(x => x.Selected).ToArray()); } }

    public void Done()
    {
      this.Close();
    }
    
    public void SetChoice(int index, int choice)
    {
      _choices[index].Selected = (EffectOption)choice;
    }

    class SetChoiceMessage
    {
      public int Index { get; set; }
      public int Choice { get; set; }
    }

    public override void ReceiveMessageType(string type, string jsonMessage)
    {
      switch (type)
      {
        case "SetChoice":
          var message = JsonConvert.DeserializeObject<SetChoiceMessage>(jsonMessage);
          SetChoice(message.Index, message.Choice);
          break;
        case "Done":
          Done();
          break;
        default:
          throw new Exception($"Unknown message type: {type}");
      }
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<IEffectChoice> choices, string text);
    }
  }
}