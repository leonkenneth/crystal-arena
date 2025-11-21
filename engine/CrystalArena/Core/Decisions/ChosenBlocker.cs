namespace CrystalArena.Decisions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Infrastructure;

    [Copyable, Serializable]
    public class ChosenBlocker : ISerializable
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
    }
}
