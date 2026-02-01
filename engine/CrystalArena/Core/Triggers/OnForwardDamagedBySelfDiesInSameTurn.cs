using System.Collections.Generic;

namespace CrystalArena.Triggers
{
    using System;
    using Events;
    using Infrastructure;

    public class OnForwardDamagedBySelfDiesInSameTurn : Trigger, IReceive<DamageDealtEvent>
    {
        private readonly List<Card> _damagedCards = new List<Card>();

        public OnForwardDamagedBySelfDiesInSameTurn() { }

        public void Receive(DamageDealtEvent message)
        {
            if (!(message.Receiver is Card))
                return;

            var receiver = (Card)message.Receiver;

            if (message.Source == OwningCard)
            {
                _damagedCards.Add(receiver);
            }
        }

        public void Receive(EndOfTurnEvent e)
        {
            _damagedCards.Clear();
        }

        public void Receive(ZoneChangedEvent message)
        {
            if (
                message.To == Zone.BreakZone
                && message.From == Zone.Battlefield
                && _damagedCards.Contains(message.Card)
            )
            {
                Set(message);
            }
        }
    }
}
