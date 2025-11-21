using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading;
using Castle.Components.DictionaryAdapter.Xml;
using MsBox.Avalonia.Enums;
using ReactiveUI;

namespace CrystalArena.UserInterface.Shell
{
    using System.Windows;
    using Caliburn.Micro;
    using Infrastructure;
    using MessageBox;
    using Messages;

    public class ApiShell : IShell
    {
        public Ui Ui { get; set; }

        class UiObjectDatabase
        {
            private Dictionary<string, object> _objects = new Dictionary<string, object>();
            private int _nextId = 0;

            public string Register(object obj)
            {
                var id = _nextId.ToString();
                var type = obj.GetType();
                var fullId = Encode($"{type.FullName}:{id}");
                _objects.Add(fullId, obj);
                _nextId++;
                return fullId;
            }

            public object Get(string id)
            {
                return _objects[id];
            }

            public Dictionary<string, string> InspectOids()
            {
                var result = new Dictionary<string, string>();
                foreach (var key in _objects.Keys)
                {
                    result.Add(key, _objects[key].GetType().FullName);
                }

                return result;
            }

            private string Encode(string plainText)
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainTextBytes);
            }
        }

        public object Screen { get; set; }
        public object Dialog { get; set; }
        public CallbackableMessageBox? MessageBox { get; set; }
        public RemoteCallbackable? CurrentDialog
        {
            get { return _remoteCallbackables.FirstOrDefault(); }
        }

        public object? CurrentSelectTargetDialog
        {
            get => GetCurrentSelectTargetDialog();
        }

        private InteractionState? _interactionState;
        private List<RemoteCallbackable> _remoteCallbackables = new List<RemoteCallbackable>();
        private UiObjectDatabase _uiObjectDatabase = new UiObjectDatabase();

        public void ChangeScreen(
            object screen,
            bool blockUntilClosed = false,
            bool shouldClosePrevious = false
        )
        {
            Screen = screen;
        }

        private object? GetCurrentSelectTargetDialog()
        {
            var dialogHost = Screen as IIsDialogHost;
            var currentDialog = dialogHost
                ?.GetAllDialogs()
                .FirstOrDefault(x => x is SelectTarget.ViewModel);

            if (currentDialog != null)
            {
                return currentDialog;
            }

            if (Dialog is SelectTarget.ViewModel selectTargetDialog)
            {
                return selectTargetDialog;
            }

            return null;
        }

        public void ShowDialog(
            object dialog,
            DialogType type = DialogType.Large,
            InteractionState? interactionState = null,
            bool wait = false
        )
        {
            var dialogHost = Screen as IIsDialogHost;

            if (dialogHost == null)
            {
                Dialog = dialog;
            }
            else
            {
                dialogHost.AddDialog(dialog, type);
            }

            var revert = ChangeMode(interactionState);
            var currentDialog = new CallbackableDialog() { ViewModel = dialog };
            _remoteCallbackables.Add(currentDialog);
            ((IClosable)dialog).Closed += delegate
            {
                if (dialogHost == null)
                {
                    Dialog = null;
                }
                else
                {
                    dialogHost.RemoveDialog(dialog);
                }

                _remoteCallbackables.Remove(currentDialog);
                ChangeMode(revert);
            };

            if (wait)
            {
                currentDialog.WaitCallback();
            }
        }

        public ButtonResult ShowMessageBox(
            string message,
            ButtonEnum buttons,
            DialogType type = DialogType.Large,
            string title = ""
        )
        {
            MessageBox = new CallbackableMessageBox()
            {
                Message = message,
                Buttons = buttons.ToString(),
                Title = title,
            };
            _remoteCallbackables.Add(MessageBox);
            var result = MessageBox.WaitCallback();
            _remoteCallbackables.Remove(MessageBox);
            MessageBox = null;

            return result;
        }

        public void ShowModalDialog(
            object dialog,
            DialogType type = DialogType.Large,
            InteractionState? interactionState = null
        )
        {
            ShowDialog(dialog, type, interactionState, true);
        }

        public bool HasFocus(object dialog)
        {
            throw new NotImplementedException();
        }

        public void CloseAllDialogs()
        {
            throw new NotImplementedException();
        }

        public object ToJson()
        {
            var screen = Screen as ViewModelBase;
            if (screen == null)
            {
                return new { Loaded = false };
            }

            return new
            {
                Loaded = true,
                Id = Ui.GameId,
                Screen = screen.ToJson(),
                MessageBox,
                CurrentDialog = CurrentDialog?.ToJson(),
            };
        }

        public object AlternativeToJson()
        {
            var screen = Screen as ViewModelBase;
            if (screen == null)
            {
                return "No screen set or " + "screen is not a viewmodel.";
            }

            return new
            {
                Screen = screen.AlternativeToJson(),
                MessageBox,
                CurrentDialog = CurrentDialog?.ToJson(),
            };
        }

        public Dictionary<string, string> InspectOids()
        {
            return _uiObjectDatabase.InspectOids();
        }

        public void ProcessCallback(string callbackId, string result)
        {
            var callbackable = _remoteCallbackables.Find(x => x.CallbackId == callbackId);
            callbackable?.Callback(result);
        }

        public string RegisterOrGetOid(ViewModelBase viewModelBase)
        {
            return _uiObjectDatabase.Register(viewModelBase);
        }

        public void ProcessOidCallback(string oid, string result)
        {
            var obj = _uiObjectDatabase.Get(oid);
            if (obj is ViewModelBase viewModelBase)
            {
                viewModelBase.ReceiveJSONMessage(result);
            }
        }

        private InteractionState? ChangeMode(InteractionState? interactionState)
        {
            if (interactionState.HasValue)
            {
                var revert = _interactionState;
                _interactionState = interactionState.Value;

                Ui.Publisher.Publish(new UiInteractionChanged { State = interactionState.Value });

                return revert;
            }

            return null;
        }
    }
}
