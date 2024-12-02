namespace CrystalArena.Tests.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using AI;
  using Decisions;
  using Modifiers;
  using Xunit;
  using log4net.Config;

  public abstract class Scenario : IDisposable
  {
    protected static readonly IoC Container = new IoC(IoC.Configuration.Test);

    protected Scenario(bool player1ControlledByScript = true, bool player2ControlledByScript = true,
      SearchParameters searchParameters = null)
    {
      var settings = Settings.Load();

      var player1Controller = player1ControlledByScript ? PlayerType.Scenario : PlayerType.Machine;
      var player2Controller = player2ControlledByScript ? PlayerType.Scenario : PlayerType.Machine;
      searchParameters = searchParameters ?? settings.GetSearchParameters();

      var p = GameParameters.Scenario(player1Controller, player2Controller, searchParameters);
      Game = new Game(p);
      Game.Players.Starting = Game.Players.Player1;
    }

    protected Game Game { get; private set; }
    protected Player P1 { get { return Game.Players.Player1; } }
    protected Player P2 { get { return Game.Players.Player2; } }
    protected Combat Combat { get { return Game.Combat; } }

    public virtual void Dispose() {}

    protected Deck GetDeck(string name)
    {
      return DeckFile.Read("/Users/leo/code/arena/CrystalArena/CrystalArena.Tests/" + name);
    }

    protected ScenarioStep At(Step step, int turn = 1)
    {
      return new ScenarioStep(step, turn, Game);
    }

    protected void MainDeck(Player player, params ScenarioCard[] cards)
    {
      var library = (MainDeck) player.MainDeck;

      foreach (var scenarioCard in cards.Reverse())
      {
        scenarioCard.Initialize(name =>
          {
            var card = CrystalArena.Cards.Create(name);
            card.Initialize(player, Game);
            library.AddToFront(card);

            return card;
          });
      }
    }

    protected void RemoveFromPlayMainDeck(Player player)
    {
      var library = (MainDeck) player.MainDeck;

      foreach (var card in library.ToList())
      {
        card.RemoveFromPlay(null);
      }
    }

    protected void Battlefield(Player player, params ScenarioCard[] cards)
    {
      foreach (var scenarioCard in cards)
      {
        scenarioCard.Initialize(name =>
          {
            var card = CrystalArena.Cards.Create(name);
            card.Initialize(player, Game);

            player.PutCardToBattlefield(card);
            card.HasSummoningSickness = false;

            if (scenarioCard.IsTapped)
              card.Tap();

            foreach (var monster in scenarioCard.Monsters)
            {
              monster.Initialize(monsterName =>
                {
                  var monsterCard = CrystalArena.Cards.Create(monsterName);
                  monsterCard.Initialize(player, Game);
                  EnchantCard(card, monsterCard);
                  AddCounters(monster, monsterCard);
                  return monsterCard;
                });
            }

            foreach (var equipment in scenarioCard.Equipments)
            {
              equipment.Initialize(equipmentName =>
                {
                  var equipmentCard = CrystalArena.Cards.Create(equipmentName);
                  equipmentCard.Initialize(player, Game);
                  player.PutCardToBattlefield(equipmentCard);
                  EquipCard(card, equipmentCard);
                  AddCounters(equipment, equipmentCard);
                  return equipmentCard;
                });
            }

            foreach (var tracked in scenarioCard.TrackedBy)
            {
              tracked.Initialize(trackerName =>
                {
                  var tracker = CrystalArena.Cards.Create(trackerName);
                  tracker.Initialize(player, Game);
                  player.PutCardToBattlefield(tracker);
                  TrackCard(card, tracker);
                  return tracker;
                });
            }

            AddCounters(scenarioCard, card);

            return card;
          });
      }
    }

    private static void AddCounters(ScenarioCard scenarioCard, Card card)
    {
      if (scenarioCard.HasCounters)
      {
        var p = new ModifierParameters
          {
            SourceCard = card,
          };

        var counters = new AddCounters(() =>
        {
          if (scenarioCard.Counters.Type == CounterType.PowerToughness)
          {
            return new PowerToughness(1, 1);
          }
        
          return new SimpleCounter(scenarioCard.Counters.Type);
        }, scenarioCard.Counters.Count);

        card.AddModifier(counters, p);
      }
    }

    protected IEnumerable<Card> Permanents(Player controller, params string[] cardNames)
    {
      foreach (var cardName in cardNames)
      {
        var battlefield = (Battlefield) controller.Battlefield;
        var card = CrystalArena.Cards.Create(cardName);
        card.Initialize(controller, Game);
        battlefield.Add(card);
        yield return card;
      }
    }

    protected ScenarioCard C(string name)
    {
      return name;
    }

    protected Card C(ScenarioCard scenarioCard)
    {
      return scenarioCard;
    }
    
    protected ITarget[] Ts(params ScenarioCard[] targetCards)
    {
      return targetCards.Select(x => (ITarget)x.Card).ToArray();
    }

    protected ScenarioEffect E(ScenarioCard scenarioCard)
    {
      return new ScenarioEffect {Effect = () => Game.Stack.First(x => x.Source.OwningCard == scenarioCard.Card)};
    }

    protected void EnableLogging(string level)
    {
      var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
        String.Format("CrystalArena.Tests.Infrastructure.Logger.{0}.xml", level));

      XmlConfigurator.Configure(stream);
    }


    protected void Equal<T>(T expected, T actual)
    {
      Assert.Equal(expected, actual);
    }

    private void EquipCard(Card card, Card equipment)
    {
      equipment.EquipWithoutPayingCost(card);
    }

    protected void TrackCard(Card card, Card tracker)
    {
      card.Attach(tracker);
    }

    protected void Exec(params ScenarioStep[] steps)
    {
      const int untilTurn = 5;
      Game.Scenario.Define(steps);
      RunGame(untilTurn);
      AssertAllStepsHaveRun(steps);
    }

    protected void False(bool condition, string message = null)
    {
      Assert.False(condition, message);
    }

    protected void BreakZone(Player player, params ScenarioCard[] cards)
    {
      var breakZone = (BreakZone) player.BreakZone;

      foreach (var scenarioCard in cards)
      {
        scenarioCard.Initialize(name =>
          {
            var card = CrystalArena.Cards.Create(name);
            card.Initialize(player, Game);
            breakZone.AddToEnd(card);

            return card;
          });
      }
    }

    protected void Hand(Player player, params ScenarioCard[] cards)
    {
      var hand = (Hand) player.Hand;

      foreach (var scenarioCard in cards)
      {
        scenarioCard.Initialize(name =>
          {
            var card = CrystalArena.Cards.Create(name);
            card.Initialize(player, Game);
            hand.Add(card);

            return card;
          });
      }
    }

    protected void Null(object obj)
    {
      Assert.Null(obj);
    }

    protected virtual void RunGame(int maxTurnCount)
    {
      Game.Start(maxTurnCount, skipPreGame: true);
    }

    protected void True(bool condition, string message = null)
    {
      Assert.True(condition, message);
    }

    protected void UnEquipCard(Card card, Card equipment)
    {
      card.Detach(equipment);
    }

    private static void AssertAllStepsHaveRun(IEnumerable<ScenarioStep> steps)
    {
      foreach (var stepCommands in steps)
      {
        stepCommands.AssertAllWereExecuted();
      }
    }

    private static void EnchantCard(Card card, Card monster)
    {
      monster.EnchantWithoutPayingCost(card);
    }
  }
}