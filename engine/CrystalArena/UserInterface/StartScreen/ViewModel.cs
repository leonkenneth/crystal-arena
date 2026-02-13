namespace CrystalArena.UserInterface.StartScreen
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using Infrastructure;
    using Media;
    using SelectDeck;
    using Deck = CrystalArena.Deck;

    public class ViewModel : ViewModelBase
    {
        private string YourName => Settings.Readonly.YourName;

        public string Version
        {
            get
            {
                return FileVersionInfo
                    .GetVersionInfo(Assembly.GetExecutingAssembly().Location)
                    .ProductVersion;
            }
        }

        public string CardCount
        {
            get { return string.Format("{0} cards", Cards.Count); }
        }

        public void Test()
        {
            //Ui.Shell;
        }

        public void Play()
        {
            this.PlayRandom();
            return;
            SelectDeck.ViewModel selectDeck1 = null;
            SelectDeck.ViewModel selectDeck2 = null;

            var configuration1 = new Configuration
            {
                ScreenTitle = "Select your deck",
                ForwardText = "Next",
                PreviousScreen = this,
                Forward = (deck1) =>
                {
                    if (selectDeck2 == null)
                    {
                        selectDeck2 = ViewModels.SelectDeck.Create(
                            new Configuration
                            {
                                ScreenTitle = "Select your opponent deck",
                                ForwardText = "Start the game",
                                PreviousScreen = selectDeck1,
                                Forward = (deck2) =>
                                {
                                    try
                                    {
                                        var mp = MatchParameters.Default(
                                            player1: new PlayerParameters
                                            {
                                                Name = YourName,
                                                AvatarId = RandomEx.Next(),
                                                Deck = deck1,
                                            },
                                            player2: new PlayerParameters
                                            {
                                                Name = NameGenerator.GenerateRandomName(
                                                    MediaMainDeck.GetPlayerUnitNames()
                                                ),
                                                AvatarId = RandomEx.Next(),
                                                Deck = deck2,
                                            },
                                            isTournament: false
                                        );

                                        Ui.Match = new Match(Ui, mp);
                                        Ui.Match.Start();
                                    }
                                    catch (Exception ex)
                                    {
                                        HandleException(ex);
                                    }

                                    Shell.ChangeScreen(this);
                                },
                            }
                        );
                    }

                    Shell.ChangeScreen(selectDeck2);
                },
            };

            selectDeck1 = ViewModels.SelectDeck.Create(configuration1);
            Shell.ChangeScreen(selectDeck1);
        }

        public void PlayRandom(bool onlyBots = false)
        {
            var decks = ChooseRandomDecks();
            Play(decks[0], decks[1], onlyBots);
        }

        public void Play(Deck deck1, Deck deck2, bool onlyBots = false)
        {
            try
            {
                var mp = MatchParameters.Default(
                    player1: new PlayerParameters
                    {
                        Name = YourName,
                        AvatarId = RandomEx.Next(),
                        Deck = deck1,
                    },
                    player2: new PlayerParameters
                    {
                        Name = NameGenerator.GenerateRandomName(MediaMainDeck.GetPlayerUnitNames()),
                        AvatarId = RandomEx.Next(),
                        Deck = deck2,
                    },
                    isTournament: false
                );

                Ui.Match = new Match(Ui, mp);
                var player1Controller = onlyBots ? PlayerType.Machine : PlayerType.Human;
                var player2Controller = PlayerType.Machine;
                Ui.Match.Start(player1Controller, player2Controller);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            Shell.ChangeScreen(this);
        }
        
        public void ResumeGame(SavedGame savedGame)
        {
            try
            {
                var savedMatch = new SavedMatch
                {
                    SavedGame = savedGame
                };
                var mp = MatchParameters.Load(savedMatch, false);

                Ui.Match = new Match(Ui, mp);
                var player1Controller = PlayerType.Human;
                var player2Controller = PlayerType.Machine;
                Ui.Match.Start(player1Controller, player2Controller);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            Shell.ChangeScreen(this);
        }

        private Deck[] ChooseRandomDecks()
        {
            return new[] { DeckLibrary.RandomDeck(), DeckLibrary.RandomDeck() };
        }

        public interface IFactory
        {
            ViewModel Create();
        }

        public override object ToJson()
        {
            return "start screen still";
        }
    }
}
