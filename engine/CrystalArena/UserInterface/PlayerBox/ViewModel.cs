namespace CrystalArena.UserInterface.PlayerBox
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using Infrastructure;
  using Messages;

  public class ViewModel : ViewModelBase, IDisposable, IReceive<UiInteractionChanged>
  {
    private readonly Timer _timer;
    private bool _canChangeSelection;

    public ViewModel(Player player)
    {      
      Player = player;
      Emblems = player.Emblems.Select(x => x.Text).ToList();    

      IncreaseLifeAnimation = Animation.Create();
      DecreaseLifeAnimation = Animation.Create();
      
      Update(enableAnimations: false);

      _timer = new Timer(delegate { Update(); }, null,
        TimeSpan.FromMilliseconds(20),
        TimeSpan.FromMilliseconds(20));      
      
    }

    public Player Player { get; private set; }

    public virtual int HandCount { get; protected set; }
    public virtual int MainDeckCount { get; protected set; }
    public virtual int BreakZoneCount { get; protected set; }    
    public virtual int Life { get; protected set; }
    public virtual bool IsActive { get; protected set; }    
    public virtual int LifeChange { get; protected set; }
    public virtual List<string> Emblems { get; protected set; }

    public Animation IncreaseLifeAnimation { get; private set; }
    public Animation DecreaseLifeAnimation { get; private set; }    
    
    public virtual string EmblemString { get; set; }

    public override object ToJson()
    {
      return new
      {
        HandCount,
        MainDeckCount,
        BreakZoneCount,
        Life,
        IsActive,
        LifeChange,
        Emblems,
        PlayerName = Player.Name,
        PlayerId = Player.Id,
        PlayerType = Player.Type.ToString(),
        Oid = base.ToJsonWithOid()
      };
    }

    public void Dispose()
    {
      _timer.Dispose();
    }

    public void Receive(UiInteractionChanged message)
    {
      _canChangeSelection = message.State == InteractionState.SelectTarget;  
    }

    private void Update(bool enableAnimations = true)
    {
      Update(() => HandCount != Player.Hand.Count, () => HandCount = Player.Hand.Count);
      Update(() => MainDeckCount != Player.MainDeck.Count, () => MainDeckCount = Player.MainDeck.Count);
      Update(() => BreakZoneCount != Player.BreakZone.Count, () => BreakZoneCount = Player.BreakZone.Count);      
      Update(() => Emblems.Count != Player.Emblems.Count(), () => Emblems = Player.Emblems.Select(x => x.Text).ToList());
      
      Update(() => Life != Player.Life, () =>
        {
          var oldLife = Life;                                        
          Life = Player.Life;

          LifeChange = Math.Abs(oldLife - Life);
          
          // at the time ctor is called
          // animations are not yet ready 
          // to be executed so disable them
          if (enableAnimations)
            AnimateLifeChange(oldLife, Player.Life);
        });

      Update(() => IsActive = Player.IsActive, () => IsActive = Player.IsActive);
    }

    private void AnimateLifeChange(int oldLife, int newLife)
    {
      if (oldLife > newLife)
      {            
        DecreaseLifeAnimation.Start();
      }
      else
      {            
        IncreaseLifeAnimation.Start();
      }
    }

    private static void Update(Func<bool> condition, Action update)
    {
      if (condition()) update();
    }

    public void ChangeSelection()
    {
      if (!_canChangeSelection)
        return;
      
      Publisher.Publish(
        new SelectionChanged {Selection = Player});
    }
    
    public override void ReceiveMessageType(string type, string message)
    {
      switch (type)
      {
        case "ChangeSelection":
          ChangeSelection();
          break;
        default:
          throw new ArgumentException($"Unknown message type: {type}");
      }
    }

    public interface IFactory
    {
      ViewModel Create(Player player);
      void Release(ViewModel viewModel);
    }
  }
}