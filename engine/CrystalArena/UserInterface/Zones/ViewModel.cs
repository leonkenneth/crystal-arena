namespace CrystalArena.UserInterface.Zones
{
    using System;

    public class ViewModel : ViewModelBase, IDisposable
    {
        public BreakZone.ViewModel OpponentsBreakZone { get; private set; }
        public Hand.ViewModel OpponentsHand { get; private set; }
        public BreakZone.ViewModel YourBreakZone { get; private set; }
        public Hand.ViewModel YourHand { get; private set; }
        public MainDeck.ViewModel YourMainDeck { get; private set; }
        public MainDeck.ViewModel OpponentsMainDeck { get; private set; }
        public LimitBreak.ViewModel YourLimitBreak { get; private set; }
        public LimitBreak.ViewModel OpponentsLimitBreak { get; private set; }

        public RemoveFromPlay.ViewModel YourRemoveFromPlay { get; private set; }
        public RemoveFromPlay.ViewModel OpponentsRemoveFromPlay { get; private set; }
        public DamageZone.ViewModel YourDamageZone { get; private set; }
        public DamageZone.ViewModel OpponentsDamageZone { get; private set; }

        public override void Initialize()
        {
            OpponentsHand = ViewModels.Hand.Create(Players.Computer);
            YourHand = ViewModels.Hand.Create(Players.Human);

            OpponentsBreakZone = ViewModels.BreakZone.Create(Players.Computer);
            YourBreakZone = ViewModels.BreakZone.Create(Players.Human);

            OpponentsMainDeck = ViewModels.MainDeck.Create(Players.Computer);
            YourMainDeck = ViewModels.MainDeck.Create(Players.Human);

            OpponentsRemoveFromPlay = ViewModels.RemoveFromPlay.Create(Players.Computer);
            YourRemoveFromPlay = ViewModels.RemoveFromPlay.Create(Players.Human);

            OpponentsDamageZone = ViewModels.DamageZone.Create(Players.Computer);
            YourDamageZone = ViewModels.DamageZone.Create(Players.Human);

            OpponentsLimitBreak = ViewModels.LimitBreak.Create(Players.Computer);
            YourLimitBreak = ViewModels.LimitBreak.Create(Players.Human);
        }

        public override object ToJson()
        {
            return new
            {
                OpponentsBreakZone = OpponentsBreakZone.ToJson(),
                OpponentsHand = OpponentsHand.ToJson(),
                YourBreakZone = YourBreakZone.ToJson(),
                YourHand = YourHand.ToJson(),
                YourMainDeck = YourMainDeck.ToJson(),
                OpponentsMainDeck = OpponentsMainDeck.ToJson(),
                YourRemoveFromPlay = YourRemoveFromPlay.ToJson(),
                OpponentsRemoveFromPlay = OpponentsRemoveFromPlay.ToJson(),
                YourDamageZone = YourDamageZone.ToJson(),
                OpponentsDamageZone = OpponentsDamageZone.ToJson(),
                YourLimitBreak = YourLimitBreak.ToJson(),
                OpponentsLimitBreak = OpponentsLimitBreak.ToJson(),
            };
        }

        public void Dispose()
        {
            YourHand.Dispose();
            OpponentsHand.Dispose();
            YourBreakZone.Dispose();
            OpponentsBreakZone.Dispose();
            YourMainDeck.Dispose();
            OpponentsMainDeck.Dispose();
            YourRemoveFromPlay.Dispose();
            OpponentsRemoveFromPlay.Dispose();
        }
    }
}
