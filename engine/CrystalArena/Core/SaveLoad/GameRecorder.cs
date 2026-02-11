using System.Collections.Generic;
using CrystalArena.Decisions;
using DynamicData;

namespace CrystalArena
{
    using System.IO;

    public class GameRecorder
    {
        private readonly DecisionLog _decisionLog;
        private readonly Game _game;
        private readonly IdentityManager _identityManager;

        public GameRecorder(Game game, List<string>? savedDecisions = null)
        {
            _game = game;
            _identityManager = new IdentityManager();
            _decisionLog = new DecisionLog(game, savedDecisions);
        }

        public bool IsPlayback
        {
            get { return !_decisionLog.IsAtTheEnd; }
        }

        public int CreateId(object obj)
        {
            if (_game.Ai.IsSearchInProgress)
                return -1;

            return _identityManager.GetId(obj);
        }

        public object GetObject(int id)
        {
            if (_game.Ai.IsSearchInProgress)
                return null;

            return _identityManager.GetObject(id);
        }

        public void SaveDecisionResult(DecisionResult result)
        {
            if (_game.Ai.IsSearchInProgress)
                return;

            _decisionLog.SaveResult(result);
        }

        public T LoadDecisionResult<T>() where T : DecisionResult
        {
            return _decisionLog.LoadResult<T>();
        }

        public SavedGame SaveGame()
        {
            var player1 = _game.Players.Player1;
            var player2 = _game.Players.Player2;

            var savedGame = new SavedGame
            {
                Player1 = new PlayerParameters
                {
                    Name = player1.Name,
                    AvatarId = player1.AvatarId,
                    Deck = player1.Deck,
                },
                Player2 = new PlayerParameters
                {
                    Name = player2.Name,
                    AvatarId = player2.AvatarId,
                    Deck = player2.Deck,
                },
                RandomSeed = _game.Random.Seed,
                Decisions = new DecisionLog(_game, new List<string>(_decisionLog.SavedDecisions)),
                StateCount = _game.Turn.StateCount,
            };

            return savedGame;
        }

        public void DiscardUnloadedResults()
        {
            _decisionLog.DiscardUnloadedResults();
        }
    }
}
