using System;
using System.Collections.Generic;
using CrystalArena.Decisions;

namespace CrystalArena
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;

    public class DecisionLog
    {
        private List<IDecisionResult> _savedDecisions = new List<IDecisionResult>();
        private readonly SerializationContext? _context;
        private int _currentIndex = 0;

        public DecisionLog(Game? game, List<IDecisionResult>? savedDecisions)
        {
            _savedDecisions = savedDecisions ?? new List<IDecisionResult>();
            _context = game == null ? null : new SerializationContext { Game = game };
        }

        public bool IsAtTheEnd
        {
            get { return _currentIndex == _savedDecisions.Count; }
        }

        public List<IDecisionResult>? SavedDecisions => _savedDecisions;

        public void SaveResult(IDecisionResult result)
        {
            _savedDecisions.Add(result);
            _currentIndex++;
        }

        public T LoadResult<T>() where T : IDecisionResult
        {
            if (_currentIndex >= _savedDecisions.Count)
                return default;

            var result = _savedDecisions[_currentIndex];
            _currentIndex++;
            return (T)result;
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
