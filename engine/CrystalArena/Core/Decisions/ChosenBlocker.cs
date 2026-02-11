namespace CrystalArena.Decisions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Infrastructure;

    [Copyable, Serializable]
    public class ChosenBlocker : DecisionResult
    {
        public static readonly ChosenBlocker None = new();
        public Card? Blocker;

        private ChosenBlocker() { }

        public ChosenBlocker(Card? card = null)
        {
            Blocker = card;
        }

        private ChosenBlocker(SerializationInfo info, StreamingContext context)
        {
            var ctx = (SerializationContext)context.Context;

            var cardId = (int?)info.GetValue("card", typeof(int?));

            if (cardId.HasValue)
            {
                Blocker = (Card)ctx.Recorder.GetObject(cardId.Value);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var cardId = Blocker?.Id;

            info.AddValue("card", cardId);
        }

        public override string TypeName => nameof(ChosenBlocker);

        public override void Serialize(JsonFormatter.ISerializationInfo info)
        {
            var cardId = Blocker?.Id;
            info.AddValue("card", cardId);
        }

        internal static ChosenBlocker FromObjectData(JsonFormatter.ISerializationInfo info)
        {
            var ctx = info.Context;
            var cardId = info.GetNullableInt32("card");

            if (cardId.HasValue)
            {
                var card = (Card)ctx.Recorder.GetObject(cardId.Value);
                return new ChosenBlocker(card);
            }

            return new ChosenBlocker();
        }
    }
}
