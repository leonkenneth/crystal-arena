using System.Linq;

namespace CrystalArena.Decisions
{
  using System;
  using System.Collections.Generic;
  using Effects;
  using Events;
  using UserInterface;

  public class ChoseModalEffect : Decision
  {
    private readonly Params _p = new Params();

    private ChoseModalEffect() {}

    public ChoseModalEffect(Player controller, Action<Params> setParameters) : base(controller,
      () => new UiHandler(), () => new MachineHandler(), () => new MachineHandler(), () => new PlaybackHandler())
    {
      setParameters(_p);
    }

    private abstract class Handler : DecisionHandler<ChoseModalEffect, ChosenModalEffectIndex>
    {
      protected override bool ShouldExecuteQuery { get { return true; } }
      
      protected int NumberOfChoices => D._p.Effect.ChildEffects.Count;

      public override void ProcessResults()
      {
        var chosenModalEffectIndex = Result;
        var chosenEffect = D._p.Effect.ChildEffectFactories[chosenModalEffectIndex.Indices[0]];
        D._p.TriggeredAbility.RunSelectedModalEffectParameters(chosenEffect);
      }
    }

    private class MachineHandler : Handler
    {
      public MachineHandler()
      {
        Result = new ChosenModalEffectIndex();
      }

      protected override void ExecuteQuery()
      {
        var chosen = Game.Random.RollADice(NumberOfChoices);
        Result = new ChosenModalEffectIndex(chosen);
      }
    }

    public class Params
    {
      public ModalEffect Effect;
      public TriggeredAbility TriggeredAbility;
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (ChosenModalEffectIndex) Game.Recorder.LoadDecisionResult();
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var dialog = Ui.Dialogs.SelectAbility.Create(D._p.Effect.ChildEffects.Select(e => new CardText(e.Text)), false);
        Ui.Shell.ShowModalDialog(dialog, DialogType.Small, InteractionState.SelectTarget);

        Result = new ChosenModalEffectIndex(dialog.SelectedIndex);
      }
    }
  }
}