namespace CrystalArena.UserInterface.SelectDeck
{
    using System;
    using Deck = CrystalArena.Deck;

    public class Configuration
    {
        public string ScreenTitle { get; set; }
        public string ForwardText { get; set; }
        public Action<Deck> Forward { get; set; }
        public object PreviousScreen { get; set; }
    }
}
