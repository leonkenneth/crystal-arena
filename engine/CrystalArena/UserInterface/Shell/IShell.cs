using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MsBox.Avalonia.Enums;

namespace CrystalArena.UserInterface.Shell
{
  using System.Windows;

  public interface IShell
  {
    public Ui Ui { get; set; }
    public object CurrentSelectTargetDialog { get; set; }
    void ChangeScreen(object screen, bool blockUntilClosed = false, bool shouldClosePrevious = false);
    void ShowDialog(object dialog, DialogType type = DialogType.Large, InteractionState? interactionState = null, bool wait = false);

    ButtonResult ShowMessageBox(string message, ButtonEnum buttons, DialogType type = DialogType.Large,
      string title = "");

    void ShowModalDialog(object dialog, DialogType type = DialogType.Large, InteractionState? interactionState = null);

    bool HasFocus(object dialog);
    void CloseAllDialogs();

    object ToJson();
    void ProcessCallback(string id, string result);
    string RegisterOrGetOid(ViewModelBase viewModelBase);
    void ProcessOidCallback(string oid, string result);
    object AlternativeToJson();
    Dictionary<string, string> InspectOids();
  }
}