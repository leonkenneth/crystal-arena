using System;
using System.Collections.Generic;
using CrystalArena.Decisions;

namespace CrystalArena
{
    public class DecisionLog
    {
        private List<string> _savedDecisions = new List<string>();
        private readonly SerializationContext? _context;
        private int _currentIndex = 0;
        private readonly JsonFormatter _serializer;

        public DecisionLog(Game game, List<string>? savedDecisions)
        {
            _savedDecisions = savedDecisions ?? new List<string>();
            _context = new SerializationContext { Game = game };
            _serializer = new JsonFormatter(_context);
        }

        public bool IsAtTheEnd => _currentIndex == _savedDecisions.Count;

        public List<string>? SavedDecisions => _savedDecisions;

        public void SaveResult(DecisionResult result)
        {
            var serializedDecisionResult = _serializer.Serialize(result);
            _savedDecisions.Add(serializedDecisionResult);
            _currentIndex++;
        }

        public T LoadResult<T>()
            where T : DecisionResult
        {
            if (_currentIndex >= _savedDecisions.Count)
                throw new ArgumentException("There are no saved decisions left.");

            var result = _savedDecisions[_currentIndex];
            _currentIndex++;
            var decisionResult = _serializer.Deserialize(result);
            return (T)decisionResult;
        }

        public void DiscardUnloadedResults()
        {
            // Truncate savedDecisions to currentIndex size
            _savedDecisions = _savedDecisions.GetRange(0, _currentIndex);
        }

        public void ResetIndex()
        {
            _currentIndex = 0;
        }
    }
}
