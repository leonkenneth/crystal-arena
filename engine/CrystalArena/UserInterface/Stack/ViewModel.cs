using System.Linq;

namespace CrystalArena.UserInterface.Stack
{
  using System;
  using System.Collections.Generic;
  using Caliburn.Micro;
  using Infrastructure;
  using Messages;

  public class ViewModel : ViewModelBase, IReceive<UiInteractionChanged>
  {
    private readonly BindableCollection<Effect> _effects = new BindableCollection<Effect>();
    private Action<Effect> _select = delegate { };

    public IEnumerable<Effect> Effects { get { return _effects; } }

    public void Receive(UiInteractionChanged message)
    {
      switch (message.State)
      {
        case (InteractionState.SelectTarget):
          {
            _select = ChangeSelection;
            break;
          }
        case (InteractionState.Disabled):
          {
            _select = delegate { };
            break;
          }
      }
    }

    public override object ToJson()
    {
      return new
      {
        Effects = _effects.Select(e => EffectToJson(e))
      };
    }

    private object? EffectToJson(Effect effect)
    {
      switch (effect.Source.GetType().Name)
      {
        case "TriggeredAbility":
        case "ActivatedAbility":
          var ability = effect.Source as Ability;
          return new
          {
            Type = ability.GetType().Name,
            Text = ability.Text.ToString(),
            Card = ViewModels.Card.Create(ability.SourceCard).ToJson(),
            Targets = effect.Targets.Select(t => t.TargetTypeAndId()).ToList(),
            ControllerId = effect.Controller.Id,
          };
        case "CastRule":
          var castRule = effect.Source as CastRule;
          return new
          {
            Type = "CastRule",
            Text = castRule.SourceCard.Text.ToString(),
            Card = ViewModels.Card.Create(castRule.SourceCard).ToJson(),
            Targets = effect.Targets.Select(t => t.TargetTypeAndId()).ToList(),
            ControllerId = effect.Controller.Id,
          };
        default:
          throw new Exception("Effect source not recognized.");
      }
    }

    public override void Initialize()
    {
      foreach (var effect in Game.Stack)
      {
        _effects.Add(effect);
      }
            
      Game.Stack.EffectAdded += OnEffectAdded;
      Game.Stack.EffectRemoved += OnEffectRemoved;
    }

    private void OnEffectRemoved(object sender, StackChangedEventArgs e)
    {
      _effects.Remove(e.Effect);
    }

    private void OnEffectAdded(object sender, StackChangedEventArgs e)
    {
      _effects.Add(e.Effect);
    }

    public void Select(Effect effect)
    {
      _select(effect);
    }

    public void ChangePlayersInterestTarget(ITarget target, bool hasLostInterest)
    {
      if (target.IsPlayer())
        return;

      var card = target.IsCard() ? target.Card() : target.Effect().Source.OwningCard;

      var message = new PlayersInterestChanged
        {
          Visual = card,
          HasLostInterest = hasLostInterest,
        };

      Publisher.Publish(message);
    }

    public void ChangePlayersInterest(Effect effect, bool hasLostInterest)
    {
      var message = new PlayersInterestChanged
        {
          Visual = effect.Source,
          HasLostInterest = hasLostInterest,
          Target = effect.Target
        };

      Publisher.Publish(message);
    }

    private void ChangeSelection(Effect effect)
    {
      Publisher.Publish(
        new SelectionChanged {Selection = effect});
    }
  }
}