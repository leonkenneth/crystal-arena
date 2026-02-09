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
        private readonly List<IDecisionResult> _savedDecisions = new List<IDecisionResult>();
        private readonly SerializationContext _context;
        private int _currentIndex = 0;

        public DecisionLog(Game game, List<IDecisionResult>? savedDecisions)
        {
            _savedDecisions = savedDecisions ?? new List<IDecisionResult>();
            _context = new SerializationContext { Game = game };
        }

        public bool IsAtTheEnd
        {
            get { return _currentIndex == _savedDecisions.Count; }
        }

        public void SaveResult(IDecisionResult result)
        {
            _savedDecisions.Add(result);
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
            throw new NotImplementedException();
        }
    }
}
