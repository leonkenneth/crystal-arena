namespace CrystalArena.Decisions
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class ChosenOptions : DecisionResult
    {
        private readonly List<object> _options = new List<object>();

        public ChosenOptions(params object[] options)
        {
            _options.AddRange(options);
        }

        public IList<object> Options
        {
            get { return _options; }
        }

        public override string TypeName => nameof(ChosenOptions);

        public override void Serialize(JsonFormatter.ISerializationInfo info)
        {
            info.AddValue("options", _options);
        }

        internal static ChosenOptions FromObjectData(JsonFormatter.ISerializationInfo info)
        {
            var options = (List<object>)info.GetValue("options", typeof(List<object>));
            return new ChosenOptions(options.ToArray());
        }
    }
}
