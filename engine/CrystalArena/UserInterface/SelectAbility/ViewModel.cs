using System.Linq;

namespace CrystalArena.UserInterface.SelectAbility
{
    using System.Collections.Generic;
    using Infrastructure;

    public class ViewModel : ViewModelBase
    {
        private readonly List<CardText> _descriptions = new List<CardText>();
        private int _selectedIndex;
        private readonly Card _owningCard;

        public override object ToJson()
        {
            return new
            {
                Type = "SelectAbility",
                CanCancel,
                Descriptions = Descriptions.Select(d => d.ToString()),
                SelectedIndex,
                OwningCard = Ui.Dialogs.Card.Create(_owningCard).ToJson(),
                Oid = base.ToJsonWithOid(),
            };
        }

        public ViewModel(IEnumerable<CardText> descriptions, Card owningCard, bool canCancel = true)
        {
            _descriptions.AddRange(descriptions);
            _selectedIndex = -1;
            _owningCard = owningCard;
            CanCancel = canCancel;
        }

        public IEnumerable<CardText> Descriptions
        {
            get { return _descriptions; }
        }
        public bool CanCancel { get; private set; }

        public bool WasCanceled { get; private set; }

        public virtual int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                this.Close();
            }
        }

        public void Cancel()
        {
            WasCanceled = true;
            this.Close();
        }

        public void SetSelectedIndex(int index)
        {
            SelectedIndex = index;
        }

        class SetSelectedIndexMessage
        {
            public int Index { get; set; }
            public string Type { get; set; }
        }

        public override void ReceiveMessageType(string type, string message)
        {
            switch (type)
            {
                case "SetSelectedIndex":
                    var parsedMessage =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<SetSelectedIndexMessage>(
                            message
                        );
                    SetSelectedIndex(parsedMessage.Index);
                    break;
                case "Cancel":
                    Cancel();
                    break;
                default:
                    base.ReceiveMessageType(type, message);
                    break;
            }
        }

        public interface IFactory
        {
            ViewModel Create(IEnumerable<CardText> descriptions, Card owningCard, bool canCancel = true);
        }
    }
}
