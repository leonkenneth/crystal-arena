using MsBox.Avalonia.Enums;
using Sentry;

namespace CrystalArena.UserInterface
{
    using System;
    using System.Windows;
    using Infrastructure;
    using Messages;
    using Shell;

    public interface IViewModelBase
    {
        public void ReceiveMessageType(string type, string message);
    }

    public abstract class ViewModelBase : IViewModelBase
    {
        private static readonly string[] ErrorMessages = new[]
        {
            "Errors have occurred.\nWe won't tell you where or why.\nLazy programmers.",
            "The code was willing\nIt considered your request,\nBut the chips were weak.",
            "Error reduces\nYour expensive computer\nTo a simple stone.",
            "To have no errors\nWould be life without meaning\nNo struggle, no joy",
            "Rather than a beep\nOr a rude error message,\nThese words: 'Game has crashed.'",
            "A file that big?\nIt might be very useful\nBut now it is gone.",
            "I just ate your data with some fava beans and a nice chianti.",
        };

        private string? _oid;

        public Dialogs ViewModels
        {
            get { return Ui.Dialogs; }
        }

        public Publisher Publisher
        {
            get { return Ui.Publisher; }
        }

        public IShell Shell
        {
            get { return Ui.Shell; }
        }

        public Ui Ui { get; set; }

        public UiHelpers UiHelpers
        {
            get { return new UiHelpers(Ui); }
        }

        protected Game Game
        {
            get { return Match.Game; }
        }

        protected Match Match
        {
            get { return Ui.Match; }
        }

        protected Combat Combat
        {
            get { return Game.Combat; }
        }

        protected Players Players
        {
            get { return Game.Players; }
        }

        public void ChangePlayersInterest(object visual)
        {
            Ui.Publisher.Publish(new PlayersInterestChanged { Visual = visual });
        }

        public void ChangePlayersInterest(Card card)
        {
            Ui.Publisher.Publish(new PlayersInterestChanged { Visual = card });
        }

        public void ChangePlayersInterest(CardViewModel card)
        {
            Ui.Publisher.Publish(new PlayersInterestChanged { Visual = card });
        }

        protected void SaveGame()
        {
            if (Match == null)
                return;

            var saveFileHeader = new SaveFileHeader();
            object gameData;

            saveFileHeader.Description = string.Format("Single match, {0}", Match.Description);
            gameData = Match.Save();

            SavedGames.Write(saveFileHeader, gameData);
        }

        protected void HandleException(Exception ex)
        {
            SentrySdk.CaptureException(ex, scope => scope.SetTag("GameId", Ui.GameId.ToString()));
            Console.WriteLine(ex);
            LogFile.Error(ex.ToString());
            SaveGame();

            var message = ErrorMessages[RandomEx.Next(ErrorMessages.Length)];

            Shell.ShowMessageBox(
                message,
                ButtonEnum.Ok,
                DialogType.Large,
                title: "Enraged Monkey Error"
            );
        }

        public virtual void Initialize() { }

        public object ToJsonWithOid()
        {
            if (_oid == null)
            {
                _oid = Shell.RegisterOrGetOid(this);
            }
            return new { Oid = _oid };
        }

        public virtual object ToJson()
        {
            return new { YouHaveToOverrideThisSomewhere = ":)", Type = this.GetType().Name };
        }

        public virtual object AlternativeToJson()
        {
            return new { };
        }

        public void ReceiveJSONMessage(string base64JSONMessage)
        {
            var bytes = Convert.FromBase64String(base64JSONMessage);
            string decodedString = System.Text.Encoding.UTF8.GetString(bytes);
            dynamic parsedMessage = Newtonsoft.Json.JsonConvert.DeserializeObject(decodedString);
            if (parsedMessage.Type == null)
                throw new ArgumentException("Message must have a Type property");
            ReceiveMessageType(parsedMessage.Type.ToString(), decodedString);
        }

        public virtual void ReceiveMessageType(string type, string message)
        {
            throw new NotImplementedException();
        }
    }
}
