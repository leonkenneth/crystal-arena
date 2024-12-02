using System;

namespace CrystalArena.UserInterface.Priority
{
  using Decisions;
  using Infrastructure;
  using Messages;

  public class ViewModel : ViewModelBase, IReceive<PlayableSelected>
  {
    public IPlayable Playable { get; private set; }
    
    public override object ToJson()
    {
      return new
      {
        Type = "Priority",
      };
    }

    public void Receive(PlayableSelected message)
    {
      Playable = message.Playable;
      this.Close();
    }

    public override void ReceiveMessageType(string type, string message)
    {
      switch (type)
      {
        case "PlayableSelected":
          Receive(Newtonsoft.Json.JsonConvert.DeserializeObject<PlayableSelected>(message));
          break;
        case "PassPriority":
          PassPriority();
          break;
        default:
          throw new ArgumentException("Unknown message type: " + type);
      }
    }

    public void PassPriority()
    {
      Playable = new Pass();
      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create();
    }
  }
}